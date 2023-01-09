using System.Collections;
using System.Collections.Generic;
using Proto.Interfaces;
using UnityEngine;

public class YieldForEvent : IState
{
	public string StateName { get; set; }
	public IState YieldState { get; set; }
	public bool IsYield { get; set; } = false;
	public bool IsStarted { get; set; } = false;

	public DialogueScript CurrentDialogue;

	private float _innerTimer;


	public void RegisterDialogue(DialogueScript dialogue)
	{
		CurrentDialogue = dialogue;
	}

	public void OnStartState()
	{
		if (IsStarted) return;

		IsStarted = true;
	}

	public void OnState()
	{
		_innerTimer += Time.deltaTime;
		
		//TODO: Implement for events
	}

	public void OnEndState()
	{
	}
}
