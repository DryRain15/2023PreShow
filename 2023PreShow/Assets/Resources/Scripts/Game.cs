using System;
using System.Collections;
using System.Collections.Generic;
using Proto.BasicExtensionUtils;
using Proto.Interfaces;
using UnityEngine;

public class Game : MonoBehaviour, IStateContainer
{
    public static Game Instance { get; private set; }

    public List<Path> Paths = new List<Path>();
    
    public Player Player;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CurrentState?.OnState();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetState(new Stage1Play());
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetState(new Stage2Play());
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetState(new Stage3Play());
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // YieldState(new YieldForEvent(new ));
        }
    }
    
    public IState CurrentState { get; private set; }
	
    public void SetState(IState state, bool doOverride = false)
    {
        OnChangeState();
        
        CurrentState?.OnEndState();
        
        if (CurrentState?.YieldState is not null)
        {
            if (doOverride)
            {
                state.YieldState = CurrentState.YieldState;
                CurrentState = state;
                CurrentState?.OnStartState();
            }
            else
            {
                CurrentState = CurrentState.YieldState;
                CurrentState.IsYield = false;
                CurrentState.YieldState = state;
            }
        }
        else
        {
            CurrentState = state;
            CurrentState?.OnStartState();
        }
    }

    public void YieldState(IState state)
    {
        CurrentState.IsYield = true;
        state.YieldState = CurrentState;
        
        CurrentState = state;
        CurrentState?.OnStartState();
    }

    /// <summary>
    /// Use this method if any validation needed before changing state
    /// </summary>
    public void OnChangeState()
    {
    }

    private void OnValidate()
    {
        if (Paths is null || Paths.Count == 0)
        {
            return;
        }

        foreach (var path in Paths)
        {
            path.adjacentPaths = new AdjacentPathDictionary();
        }

        for (var idx = 0; idx < Paths.Count; idx++)
        {
            var path = Paths[idx];
            if (path.parentIdx >= 0 && path.parentIdx < Paths.Count)
            {
                path.Parent = Paths[path.parentIdx];
                path.adjacentPaths.Add(path.parentIdx, 0f);
                path.Parent.adjacentPaths.Add(idx, path.portion);

                foreach (var (adjIdx, por) in path.Parent.adjacentPaths)
                {
                    if (Math.Abs(por - path.portion) < Constants.Epsilon && idx != adjIdx)
                    {
                        path.adjacentPaths.Add(adjIdx, 0f);
                        Paths[adjIdx].adjacentPaths.Add(idx, 0f);
                    }
                }
            }

            path.id = idx;
            
            path.Validate();
        }
    }

    private void OnDrawGizmos()
    {
        if (CurrentState is Stage1Play s1)
        {
            foreach (var path in Paths)
            {
                path.DrawGizmos();
            }
        }
    }
}
