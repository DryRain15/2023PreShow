using System;
using System.Collections;
using System.Collections.Generic;
using Proto.BasicExtensionUtils;
using Proto.Interfaces;
using Proto.Utils;
using TwitchChatConnect.Client;
using UnityEngine;
using Random = UnityEngine.Random;

public class Stage1Play : IState
{
	public string StateName { get; set; }
	public IState YieldState { get; set; }
	public bool IsYield { get; set; } = false;
	public bool IsStarted { get; set; } = false;

	private Path _currentPath;
	private static List<Path> PathPool => Game.Instance.Paths;
	private float _progress = 0f;

	public float Speed = 3f;

	private Vector2 _position;
	private Vector2 _velocity;

	private float _innerTime = 0f;

	public void OnStartState()
	{
		if (IsStarted) return;

		IsStarted = true;
		_currentPath = Game.Instance.Paths[0];
		
		foreach (var path in Game.Instance.Paths)
		{
			path.Initiate();
		}

		var player = Game.Instance.Player;
		player.Position = _currentPath.startPoint + _progress * _currentPath.direction;
		CameraFollow.Instance.target = player.transform;

		player.PlayIntroScene(() => { TitleContainer.Instance.SetTitle("title_stage_1"); });
	}

	public void OnState()
	{
		//TODO: Implement for stage play
		var dt = Time.deltaTime;
		var axis = GlobalInputController.Instance.CurrentFrameAxis;
		_velocity += axis * Speed;
		var pos = _currentPath.startPoint + _progress * _currentPath.direction;

		_innerTime += dt;
		
		Dictionary<int, Vector2> adjIndex = new Dictionary<int, Vector2>();

		foreach (var adj in _currentPath.adjacentPaths)
		{
			var idx = adj.Key;
			var por = adj.Value;
			var adjPath = PathPool[idx];
			var dir = adjPath.direction.normalized;
			if ((_currentPath.GetPoint(por) - pos).magnitude < 0.7f && 
			    Vector2.Dot(_velocity, (_currentPath.GetPoint(por) - pos).normalized) > Constants.Epsilon)
				adjIndex.Add(idx, dir);
		}
		
		if (_velocity.magnitude < Constants.Epsilon)
			return;

		var target = _currentPath.id;
		var moveDist = Vector2.Dot(_velocity, _currentPath.direction.normalized);
		foreach (var adj in adjIndex)
		{
			var adjDist = Vector2.Dot(_velocity, adj.Value);
			var tempP = _progress + moveDist * dt * Speed / _currentPath.distance;

			if (adjDist.Abs() > moveDist.Abs() || 
			    (Math.Abs(adjDist.Abs() - moveDist.Abs()) < Constants.Epsilon && adjDist > moveDist))
			{
				moveDist = adjDist;
				target = adj.Key;
			}
			else if (target == _currentPath.id && adjDist.Abs() > 0f && tempP is <= 0f or >= 1f)
			{
				moveDist = adjDist;
				target = adj.Key;
			}
		}
		
		if (target != _currentPath.id)
		{
			var prevIdx = _currentPath.id;
			_currentPath = PathPool[target];
			_progress = _currentPath.adjacentPaths[prevIdx];

			pos = _currentPath.startPoint + _progress * _currentPath.direction.normalized;
		}
		else
		{
			pos += moveDist * dt * Speed * _currentPath.direction.normalized;
			_progress += moveDist * dt * Speed / _currentPath.distance;
			_progress = Mathf.Clamp01(_progress);
		}

		_position = (pos - _position).magnitude > dt * Speed * 2f
			? _position + (pos - _position).normalized * (dt * Speed * 2f) : pos;

		Game.Instance.Player.Position = _position;
		_velocity *= 1 - dt * 12f;
	}

	public void OnEndState()
	{
		foreach (var path in Game.Instance.Paths)
		{
			path.Eliminate();
		}
	}
}
