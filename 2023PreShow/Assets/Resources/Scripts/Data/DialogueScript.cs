using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueScript : ScriptableObject
{
    public List<DialogueEventData> DialogueEvents = new List<DialogueEventData>();

    private void OnValidate()
    {
	    for (int idx = 0; idx < DialogueEvents.Count; idx++)
	    {
		    var data = DialogueEvents[idx];
		    switch (data.Type)
		    {
			    case DialogueEventType.Speech:
				    DialogueEvents[idx] = new SpeechEventData();
				    break;
			    case DialogueEventType.Image:
				    DialogueEvents[idx] = new ImageEventData();
				    break;
			    case DialogueEventType.Shake:
				    DialogueEvents[idx] = new ShakeEventData();
				    break;
			    case DialogueEventType.Fade:
				    DialogueEvents[idx] = new FadeEventData();
				    break;
			    default:
				    break;
		    }
	    }
    }
}

[Flags]
public enum DialogueEventType{
	None,
	Speech,
	Image,
	Shake,
	Fade,
}

public class DialogueEventData
{
	protected DialogueEventType _type = DialogueEventType.None;
	public DialogueEventType Type => _type;
	public bool Wait;
}

public class SpeechEventData : DialogueEventData
{
	
	public string Text;
	public string Speaker;
	public SpeechEventData()
	{
		_type = DialogueEventType.Speech;
	}
}

public class ImageEventData : DialogueEventData
{
	public float Duration;
	public Sprite Image;
	public ImageEventData()
	{
		_type = DialogueEventType.Image;
	}
}

public class ShakeEventData : DialogueEventData
{
	public float Duration;
	public float Power;
	public ShakeEventData()
	{
		_type = DialogueEventType.Shake;
	}
}

public class FadeEventData : DialogueEventData
{
	public float Destination;
	public float Duration;
	public Color Color;
	
	public FadeEventData()
	{
		_type = DialogueEventType.Fade;
	}
}