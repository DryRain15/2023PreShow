using System.Collections;
using System.Collections.Generic;
using Proto.Interfaces;
using Proto.Utils;
using TwitchChatConnect.Client;
using TwitchChatConnect.Config;
using TwitchChatConnect.Data;
using TwitchChatConnect.Manager;
using UnityEngine;

public class Appliance : IState
{
    public string StateName { get; set; }
    public IState YieldState { get; set; }
    public bool IsYield { get; set; } = false;
    public bool IsStarted { get; set; } = false;

    public string _channelName;


    public Appliance(string channelName)
    {
        _channelName = channelName;
    }
    
    public void OnStartState()
    {
    }

    public void OnState()
    {
    }

    public void OnEndState()
    {
        
    }
    
}
