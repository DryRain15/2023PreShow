using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Flags]
public enum InputAxis
{
    N = 0x01,
    S = 0x02,
    E = 0x04,
    W = 0x08,
}

[Serializable]
public class AxisKeyDictionary : SerializableDictionary<InputAxis, KeyCode>{}

public class GlobalInputController : MonoBehaviour
{
    public static GlobalInputController Instance { get; private set; }

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

    public AxisKeyDictionary AxisKeys = new AxisKeyDictionary()
    {
        { InputAxis.N, KeyCode.W },
        { InputAxis.N|InputAxis.E, KeyCode.E },
        { InputAxis.E, KeyCode.D },
        { InputAxis.S|InputAxis.E, KeyCode.C },
        { InputAxis.S, KeyCode.X },
        { InputAxis.S|InputAxis.W, KeyCode.Z },
        { InputAxis.W, KeyCode.A },
        { InputAxis.N|InputAxis.W, KeyCode.Q }
    };

    public KeyCode confirm = KeyCode.S;
    public KeyCode cancel = KeyCode.F;
    
    public KeyCode menu = KeyCode.Escape;

    public InputAxis CurrentRawAxis;
    public Vector2 CurrentAxis;
    
    public bool ConfirmPressed { get; private set; }
    public bool CancelPressed { get; private set; }
    public bool MenuPressed { get; private set; }

    private void Update()
    {
        CurrentRawAxis = 0x00;
        CurrentAxis = Vector2.zero;

        foreach (var axisKey in AxisKeys.Where(axisKey => Input.GetKey(axisKey.Value)))
        {
            if (axisKey.Key.HasFlag(InputAxis.N))
            {
                CurrentAxis.y += 1;
                CurrentRawAxis |= InputAxis.N;
            }
            if (axisKey.Key.HasFlag(InputAxis.S))
            {
                CurrentAxis.y -= 1;
                CurrentRawAxis |= InputAxis.S;
            }
            if (axisKey.Key.HasFlag(InputAxis.E))
            {
                CurrentAxis.x += 1;
                CurrentRawAxis |= InputAxis.E;
            }
            if (axisKey.Key.HasFlag(InputAxis.W))
            {
                CurrentAxis.x -= 1;
                CurrentRawAxis |= InputAxis.W;
            }
        }
        
        CurrentAxis = CurrentAxis.normalized;
        
        ConfirmPressed = Input.GetKeyDown(confirm);
        CancelPressed = Input.GetKeyDown(cancel);
        MenuPressed = Input.GetKeyDown(menu);
    }
}
