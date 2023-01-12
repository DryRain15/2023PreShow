using System.Collections;
using System.Collections.Generic;
using Proto.Interfaces;
using Proto.Utils;
using TwitchChatConnect.Client;
using TwitchChatConnect.Config;
using TwitchChatConnect.Data;
using TwitchChatConnect.Manager;
using UnityEngine;

public class Initializing : IState
{
    public string StateName { get; set; }
    public IState YieldState { get; set; }
    public bool IsYield { get; set; } = false;
    public bool IsStarted { get; set; } = false;

    private string _userName;
    private string _userToken;
    private string _channelName;

    private TwitchConnectConfig _config;

    private float _innerTimer = 0f;
    private int _innerToken = 0;

    public Initializing(string userName, string userToken, string channelName)
    {
        _userName = userName;
        _userToken = userToken;
        _channelName = channelName;

        _config = new TwitchConnectConfig(_userName, _userToken, _channelName);
    }
    
    public void OnStartState()
    {
        TwitchChatClient.instance.Init(
            () =>
            {
                Debug.Log("Success");
                TwitchChatClient.instance.onChatCommandReceived = (command =>
                {
                    if (!Game.TwitchInputMode)
                        return;
                    
                    if (command.Command == "!입력" && command.Parameters.Length > 0)
                    {
                        int dir = -1;
                        var valid = int.TryParse(command.Parameters[0], out dir);
                        if (valid && dir is > 0 and < 10)
                        {
                            GlobalInputController.Instance.CurrentFrameRawAxis
                                = GlobalInputController.Instance.TwitchKeys[dir-1];
                            GlobalInputController.Instance.CurrentFrameAxis
                                = GlobalInputController.Instance.CurrentFrameRawAxis.GetDirection();

                            if (dir == 5)
                                GlobalInputController.Instance.ConfirmPressed = true;
                        }
                        
                    }
                    Debug.Log($"{command.Command} {command.Parameters[0] ?? "null"}");
                });
                
                Debug.Log(TwitchUserManager.Users.Count);
            },
            Debug.Log);
    }

    public void OnState()
    {
        var dt = Time.deltaTime;
        _innerTimer += dt;
    }

    public void OnEndState()
    {
        
    }
    
}
