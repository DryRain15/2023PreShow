using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Flags]
public enum InputAxis
{
    None = 0x00,
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
        { InputAxis.N, KeyCode.Keypad8 },
        { InputAxis.N|InputAxis.E, KeyCode.Keypad9 },
        { InputAxis.E, KeyCode.Keypad6 },
        { InputAxis.S|InputAxis.E, KeyCode.Keypad3 },
        { InputAxis.S, KeyCode.Keypad2 },
        { InputAxis.S|InputAxis.W, KeyCode.Keypad1 },
        { InputAxis.W, KeyCode.Keypad4 },
        { InputAxis.N|InputAxis.W, KeyCode.Keypad7 }
    };


    public InputAxis[] TwitchKeys = 
    {
        InputAxis.S|InputAxis.W,
        InputAxis.S,
        InputAxis.S|InputAxis.E,
        InputAxis.W,
        InputAxis.None,
        InputAxis.E,
        InputAxis.N|InputAxis.W,
        InputAxis.N,
        InputAxis.N|InputAxis.E,
    };

    public KeyCode confirm = KeyCode.Keypad5;
    public KeyCode cancel = KeyCode.F;
    
    public KeyCode menu = KeyCode.Escape;

    public InputAxis CurrentRawAxis;
    public Vector2 CurrentAxis;
    
    public InputAxis CurrentFrameRawAxis;
    public Vector2 CurrentFrameAxis;
    
    public bool ConfirmPressed { get; set; }
    public bool CancelPressed { get; set; }
    public bool MenuPressed { get; set; }

    public void OnUpdate()
    {
        CurrentRawAxis = 0x00;
        CurrentFrameRawAxis = 0x00;
        CurrentAxis = Vector2.zero;
        CurrentFrameAxis = Vector2.zero;

        ConfirmPressed = false;
        CancelPressed = false;
        MenuPressed = false;
        
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

        foreach (var axisKey in AxisKeys.Where(axisKey => Input.GetKeyDown(axisKey.Value)))
        {
            if (axisKey.Key.HasFlag(InputAxis.N))
            {
                CurrentFrameAxis.y += 1;
                CurrentFrameRawAxis |= InputAxis.N;
            }
            if (axisKey.Key.HasFlag(InputAxis.S))
            {
                CurrentFrameAxis.y -= 1;
                CurrentFrameRawAxis |= InputAxis.S;
            }
            if (axisKey.Key.HasFlag(InputAxis.E))
            {
                CurrentFrameAxis.x += 1;
                CurrentFrameRawAxis |= InputAxis.E;
            }
            if (axisKey.Key.HasFlag(InputAxis.W))
            {
                CurrentFrameAxis.x -= 1;
                CurrentFrameRawAxis |= InputAxis.W;
            }
        }
        
        CurrentFrameAxis = CurrentFrameAxis.normalized;
        
        ConfirmPressed = ConfirmPressed || Input.GetKeyDown(confirm);
        CancelPressed = CancelPressed || Input.GetKeyDown(cancel);
        MenuPressed = MenuPressed || Input.GetKeyDown(menu);
    }
}
