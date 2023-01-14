using System;
using System.Collections;
using System.Collections.Generic;
using Proto.BasicExtensionUtils;
using Proto.CustomDebugTool;
using Proto.Interfaces;
using UnityEngine;

public class Stage2Play : IState
{
	public string StateName { get; set; }
	public IState YieldState { get; set; }
	public bool IsYield { get; set; } = false;
	public bool IsStarted { get; set; } = false;

	public float Speed = 4f;

	private Vector2 _velocity;
	private Vector2 _position;

	private float _innerTimer = 0f;

	private Vector2 _prevAxis;

	public void OnStartState()
	{
		if (IsStarted) return;

		IsStarted = true;
		
		FadeContainer.Instance.FadeTo(0f, 2f);
		
		Game.Instance.Player.Position = Vector3.zero;
		Game.Instance.Stage2.SetActive(true);
		
		Game.Instance.CompleteText.text = Game.CompleteSentence;

		TitleContainer.Instance.SetTitle("title_stage_2");
		Game.TwitchInputMode = true;
	}

	public void OnState()
	{
		//TODO: Implement for stage play
		var dt = Time.deltaTime;
		var raw = GlobalInputController.Instance.CurrentFrameRawAxis;
		var axis = GlobalInputController.Instance.CurrentFrameAxis;

		// Input delay(deprecated)
		// if (_innerTimer > 0f)
		// {
		// 	_innerTimer -= dt;
		// }
		
		// Only Adjacent Axis input should be accepted
		if (Vector2.Dot(axis, _prevAxis).IsBetween(0.1f, 0.9f) || _prevAxis.magnitude < Constants.Epsilon)
		{
			_velocity += Speed * axis;
			_innerTimer += 0.1f;

			_prevAxis = axis;
			Game.Instance.Player.ShakeLeg(raw);
		}

		if (Game.Instance.Player.Position.magnitude > 40f)
			_velocity = -Game.Instance.Player.Position.normalized * _velocity.magnitude;
		
		Game.Instance.Player.Position += _velocity.ToVector3() * dt;
		_velocity *= 1 - dt * 5f;
	}

	public void OnEndState()
	{
		Game.Instance.Stage2.SetActive(false);
		Game.TwitchInputMode = false;
	}
}
