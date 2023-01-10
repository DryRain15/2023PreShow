using System;
using System.Collections;
using System.Collections.Generic;
using Proto.BasicExtensionUtils;
using Proto.CustomDebugTool;
using Proto.Interfaces;
using UnityEngine;

public class Stage3Play : IState
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
		
		Game.Instance.Player.Position = Vector3.zero;
	}

	public void OnState()
	{
		//TODO: Implement for stage play
		var dt = Time.deltaTime;
		var raw = GlobalInputController.Instance.CurrentFrameRawAxis;
		// Inverted input axis
		var axis = GlobalInputController.Instance.CurrentFrameAxis * -1f;

		// Input delay(deprecated)
		// if (_innerTimer > 0f)
		// {
		// 	_innerTimer -= dt;
		// }
		
		// TODO: Implement Interaction Button here

		_velocity += Speed * axis;
		_velocity = _velocity.normalized * MathF.Min(_velocity.magnitude, Speed * 4f);
		// _innerTimer += 0.1f;

		// _prevAxis = axis;
		Game.Instance.Player.ShakeLeg(raw);

		Game.Instance.Player.Position += _velocity.ToVector3() * dt;
		_velocity *= 1 - dt * 0.5f;
	}

	public void OnEndState()
	{
	}
}
