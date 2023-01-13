using System;
using System.Collections;
using System.Collections.Generic;
using Proto.BasicExtensionUtils;
using Proto.CustomDebugTool;
using Proto.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

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
	private CustomAnimator _animator;
	public Tentacles Legs;

	private void Awake()
	{
		_sr = transform.Find("Head").Find("Face").GetComponent<SpriteRenderer>();
		_animator = transform.Find("Head").Find("Face").GetComponent<CustomAnimator>();
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

	public void PlayIntroScene(Action onFinish)
	{
		StartCoroutine(IntroScene(onFinish));
	}
	
	public IEnumerator IntroScene(Action onFinish)
	{
		_animator.SetAnim("HeadIntro");
		_animator.SetNextAnim("HeadIdle");

		yield return new WaitForSeconds(0.5f);

		foreach (var legsValue in Legs.Values)
		{
			legsValue.SetAnim("TentacleIntro");
			yield return new WaitForSeconds(Random.value * 0.3f);
		}
		
		onFinish?.Invoke();
	}
	
	public void ShakeLeg(InputAxis axis)
	{
		if (!Legs.ContainsKey(axis)) return;
		
		Legs[axis].SetAnim("TentacleWhip");
	}

	public void GrabLeg(InputAxis axis)
	{
		if (!Legs.ContainsKey(axis)) return;
		
		
		Legs[axis].SetAnim("TentacleGrab");
	}

	public void ShootLeg(InputAxis axis)
	{
		if (!Legs.ContainsKey(axis)) return;
		
		
		Legs[axis].SetAnim("TentacleShoot");
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

	private void PlayAnim(string animName, bool loop = false)
	{
		_animator.SetAnim(animName, loop);
	}
}
