using System;
using System.Collections;
using System.Collections.Generic;
using Proto.CustomDebugTool;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
	public Vector3 Position
	{
		get => transform.position;
		set => transform.position = value;
	}

	private void Start()
	{
		if (Game.Instance.Player == null)
		{
			Game.Instance.Player = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Update()
	{
		CustomDebugger.Instance.AddDrawCircleGizmo(transform.position, 0.2f, Color.red);
	}
}
