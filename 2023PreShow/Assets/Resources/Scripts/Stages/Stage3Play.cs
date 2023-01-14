using System;
using System.Collections;
using System.Collections.Generic;
using Proto.BasicExtensionUtils;
using Proto.CustomDebugTool;
using Proto.Interfaces;
using TwitchChatConnect.Client;
using UnityEngine;
using Random = UnityEngine.Random;

public class Stage3Play : IState
{
	public string StateName { get; set; }
	public IState YieldState { get; set; }
	public bool IsYield { get; set; } = false;
	public bool IsStarted { get; set; } = false;

	public float Speed = 2f;

	private Vector2 _velocity;
	private Vector2 _position;

	private float _innerTimer = 0f;

	private Vector2 _prevAxis;

	public void OnStartState()
	{
		if (IsStarted) return;

		IsStarted = true;
		
		FadeContainer.Instance.FadeTo(0f, 2f);
		
		Game.Instance.Stage3.SetActive(true);
		Game.Instance.Player.Position = Vector3.zero;
		
		Game.Instance.CompleteText.text = Game.CompleteSentence;
		
		TitleContainer.Instance.SetTitle("title_stage_3");
	}

	public void OnState()
	{
		var dt = Time.deltaTime;
		var raw = GlobalInputController.Instance.CurrentFrameRawAxis;
		// Inverted input axis
		var axis = GlobalInputController.Instance.CurrentFrameAxis * -1f;

		_innerTimer += dt;

		// Input delay(deprecated)
		// if (_innerTimer > 2f)
		// {
		// 	_innerTimer = 0f;
		// {
		// 	TwitchChatClient.instance.SendChatMessage($"!입력 {Mathf.FloorToInt(Random.value * 8.9999f + 1f)}");
		// 	GlobalInputController.Instance.CurrentFrameRawAxis
		// 		= GlobalInputController.Instance.TwitchKeys[dir-1];
		// 	GlobalInputController.Instance.CurrentFrameAxis
		// 		= GlobalInputController.Instance.CurrentFrameRawAxis.GetDirection();
		//
		// 	if (dir == 5)
		// 		GlobalInputController.Instance.ConfirmPressed = true;
		// }
		// }

		_velocity += Speed * axis;
		_velocity = _velocity.normalized * MathF.Min(_velocity.magnitude, Speed * 6f);
		// _innerTimer += 0.1f;

		// _prevAxis = axis;
		Game.Instance.Player.ShootLeg(raw);
		
		if (Game.Instance.Player.Position.magnitude > 48.4f)
			_velocity = -Game.Instance.Player.Position.normalized * _velocity.magnitude;

		Game.Instance.Player.Position += _velocity.ToVector3() * dt;
		_velocity *= 1 - dt * 0.1f;
	}

	public void OnEndState()
	{
		Game.Instance.Stage3.SetActive(false);
		Game.TwitchInputMode = false;
	}
}
