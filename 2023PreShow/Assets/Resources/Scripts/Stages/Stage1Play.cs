using System;
using System.Collections;
using System.Collections.Generic;
using Proto.BasicExtensionUtils;
using Proto.CustomDebugTool;
using Proto.Interfaces;
using UnityEngine;

public class Stage1Play : IState
{
	public string StateName { get; set; }
	public IState YieldState { get; set; }
	public bool IsYield { get; set; } = false;
	public bool IsStarted { get; set; } = false;

	private Path _currentPath;
	private static List<Path> PathPool => Game.Instance.Paths;
	private float _progress = 0f;

	public float Speed = 6f;

	private Vector2 _position;

	public void OnStartState()
	{
		if (IsStarted) return;

		IsStarted = true;
		_currentPath = Game.Instance.Paths[0];

		Game.Instance.Player.Position = _currentPath.startPoint + _progress * _currentPath.direction;
	}

	public void OnState()
	{
		//TODO: Implement for stage play
		var dt = Time.deltaTime;
		var axis = GlobalInputController.Instance.CurrentAxis;
		var pos = _currentPath.startPoint + _progress * _currentPath.direction;

		Dictionary<int, Vector2> adjIndex = new Dictionary<int, Vector2>();

		foreach (var adj in _currentPath.adjacentPaths)
		{
			var idx = adj.Key;
			var por = adj.Value;
			var adjPath = PathPool[idx];
			var dir = adjPath.direction.normalized;
			if ((_currentPath.GetPoint(por) - pos).magnitude < 0.15f)
				adjIndex.Add(idx, dir);
		}
		
		//TODO: Implement Interaction Button here
		
		if (axis.magnitude < Constants.Epsilon)
			return;

		var target = _currentPath.id;
		var moveDist = Vector2.Dot(axis, _currentPath.direction.normalized);
		foreach (var adj in adjIndex)
		{
			var adjDist = Vector2.Dot(axis, adj.Value);

			Debug.Log($"{target} >> {adj.Key} : {moveDist} ~~ {adjDist}");
			if (adjDist.Abs() > moveDist.Abs() || 
			    (Math.Abs(adjDist.Abs() - moveDist.Abs()) < Constants.Epsilon && adjDist > moveDist))
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

		_position = pos;

		Game.Instance.Player.Position = _position;
	}

	public void OnEndState()
	{
	}
}
