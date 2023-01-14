using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Proto.BasicExtensionUtils;
using Proto.Interfaces;
using TMPro;
using TwitchChatConnect.Client;
using UnityEngine;

public class Game : MonoBehaviour, IStateContainer
{
    public static Game Instance { get; private set; }

    private Transform _storage;
    public Transform Storage => _storage;
    
    public Player Player;

    public string currentStateName;
    
    public string userName;
    public string userToken;
    public string channelName;
    
    public DialogueScript TestScript;
    
    public List<Path> Paths = new List<Path>();

    public GameObject Stage1;
    public GameObject Stage2;
    public GameObject Stage3;

    public TMP_Text TwitchState;

    private static bool _twitchInputMode;
    public static bool TwitchInputMode
    {
        get => _twitchInputMode;
        set
        {
            _twitchInputMode = value;
            Instance.TwitchState.text = value ? "트위치 입력 활성화" : "";
        }
    }

    public static string Word1 = "";
    public static string Word2 = "";
    public static string Word3 = "";
    public static string CompleteSentence = $"{Word1}이(가) {Word2}에서 {Word3}을(를) ...하게 한다";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        _storage = transform.Find("Storage");
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        SetState(new Initializing(
            userName, userToken, channelName
            ));
        TwitchState.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            TwitchInputMode = !TwitchInputMode;

        GlobalInputController.Instance.OnUpdate();
        
        CurrentState?.OnState();
        
        
        // if (Input.GetKeyDown(KeyCode.Alpha1))
        // {
        //     SetState(new Stage1Play());
        // }
        //
        // if (Input.GetKeyDown(KeyCode.Alpha2))
        // {
        //     SetState(new Stage2Play());
        // }
        //
        // if (Input.GetKeyDown(KeyCode.Alpha3))
        // {
        //     SetState(new Stage3Play());
        // }
        //
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     YieldState(new YieldForEvent(TestScript));
        // }
    }
    
    public IState CurrentState { get; private set; }

    public void SetState(IState state, bool doOverride = false)
    {
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
        
        OnChangeState();
    }

    public void YieldState(IState state)
    {
        CurrentState.IsYield = true;
        state.YieldState = CurrentState;
        
        CurrentState = state;
        CurrentState?.OnStartState();
    }

    /// <summary>
    /// Use this method if any validation needed after changing state
    /// </summary>
    public void OnChangeState()
    {
        currentStateName = CurrentState?.GetType().Name;
    }

    private void OnValidate()
    {
        if (Paths is null || Paths.Count == 0)
        {
            return;
        }

        for (var idx = 0; idx < Paths.Count; idx++)
        {
            var path = Paths[idx];
            path.id = idx;
            if (idx > 0)
                path.Parent = Paths[path.parentIdx];
            path.adjacentPaths = new AdjacentPathDictionary();
        }

        for (var idx = 0; idx < Paths.Count; idx++)
        {
            var path = Paths[idx];
            
            // Use Dot Product to get distance between the point and the vector
            foreach (var aP in Paths)
            {
                if (aP.id == idx)
                    continue;

                var aPsp = aP.GetPoint(0f);
                var aPep = aP.GetPoint(1f);

                var ps2Apsp = aPsp - path.startPoint;
                var ps2Apep = aPep - path.startPoint;

                if (Vector2.Dot(ps2Apsp, path.direction.GetLeftPerpendicular()).Abs()/path.direction.magnitude < 0.12f)
                {
                    var dist = Vector2.Dot(ps2Apsp, path.direction) / path.direction.sqrMagnitude;
                    if (dist is >= 0f and <= 1f)
                    {
                        path.adjacentPaths[aP.id] = dist;
                        aP.adjacentPaths[idx] = 0f;
                    }
                }
                else if (Vector2.Dot(ps2Apep, path.direction.GetLeftPerpendicular()).Abs()/path.direction.magnitude < 0.12f)
                {
                    var dist = Vector2.Dot(ps2Apep, path.direction) / path.direction.sqrMagnitude;
                    if (dist is >= 0f and <= 1f)
                    {
                        path.adjacentPaths[aP.id] = dist;
                        aP.adjacentPaths[idx] = 1f;
                    }
                }
            }
            
            path.Validate();
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var path in Paths)
        {
            path.DrawGizmos();
        }
    }
    
    public void OnApplicationQuit()
    {
        CurrentState?.OnEndState();
    }
}
