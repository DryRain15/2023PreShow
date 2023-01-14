using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewDialogueEvent", menuName = "Dialogue/Create Dialogue Event", order = 2)]
public class DialogueScript : ScriptableObject
{
	public int Count => DialogueEvents?.Count ?? 0;
	
	public bool fadeOnStart;
	
	[SerializeField]
    public List<DialogueEventData> DialogueEvents;
}

[Serializable]
public enum DialogueEventType{
	None,
	Speech,
	Image,
	Shake,
	Fade,
	Choice,
	Gather,
	Title,
	Sound,
}

[Serializable]
public class DialogueEventData
{
	[SerializeField]
	protected DialogueEventType _type = DialogueEventType.None;
	public DialogueEventType Type => _type;
	public bool Wait = true;
	public float Duration;
	
	// EmptyEvent
	public bool TextboxOff = false;
	public bool ImageOff = false;
	
	// SpeechEvent
	public string Text;
	public string Speaker;
	public float TimeMult = 1f;
	
	// ImageEvent
	public Sprite Image;
	public Rect ImageRect;
	
	// ShakeEvent
	public float Power;
	
	// FadeEvent
	public float Destination;
	public Color Color;
	
	// ChoiceEvent
	public List<string> Choices;
	
	// SoundEvent
	public AudioClip Sound;
}
