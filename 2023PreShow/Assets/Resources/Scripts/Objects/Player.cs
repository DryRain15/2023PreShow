using System;
using System.Collections;
using System.Collections.Generic;
using Proto.BasicExtensionUtils;
using Proto.CustomDebugTool;
using Proto.Utils;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class Tentacles : SerializableDictionary<InputAxis, Tentacle> { }

public class Player : MonoBehaviour
{
	public Vector3 Position
	{
		get => transform.position;
		set => transform.position = value;
	}

	// TODO: Should be changed into custom animator 
	private SpriteRenderer _sr;
	public Tentacles Legs;

	private void Awake()
	{
		_sr = transform.Find("Head").GetComponent<SpriteRenderer>();
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

		foreach (var (axis, tentacle) in Legs)
		{
			var dir = axis.GetDirection() * 0.3f;
			tentacle.transform.localPosition = dir.ToVector3(0.1f);
			tentacle.transform.localRotation = Quaternion.Euler(0, 0, dir.ToAngle());
		}
	}

	private void Update()
	{
	}
	
	public void ShakeLeg(InputAxis axis)
	{
		if (!Legs.ContainsKey(axis)) return;
		
		Legs[axis].Shake();
	}

	private void OnValidate()
	{
		foreach (var (axis, tentacle) in Legs)
		{
			var dir = axis.GetDirection() * 0.3f;
			tentacle.transform.localPosition = dir.ToVector3(0.1f);
			tentacle.transform.localRotation = Quaternion.Euler(0, 0, dir.ToAngle());
		}
	}
}
