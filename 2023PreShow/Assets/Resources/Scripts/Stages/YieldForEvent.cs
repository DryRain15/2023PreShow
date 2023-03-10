using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Proto.BasicExtensionUtils;
using Proto.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

public class YieldForEvent : IState
{
	public string StateName { get; set; }
	public IState YieldState { get; set; }
	public bool IsYield { get; set; } = false;
	public bool IsStarted { get; set; } = false;

	public DialogueScript CurrentDialogue;
	private string _currentParam;
	private Dictionary<string, Dictionary<string, string>> _textData;

	private int _currentLine = 0;
	private float _innerTimer;

	public int ChoiceCache = -1;
	public int ChoiceStack = 0;

	public Action<int> OnChoiceResult;
	public Action OnEndEvent;

	public YieldForEvent(DialogueScript dialogue)
	{
		RegisterDialogue(dialogue);
	}

	public YieldForEvent(DialogueScript dialogue, string param, Action<int> choice = null)
	{
		RegisterDialogue(dialogue);
		_currentParam = param;
		OnChoiceResult = choice;
	}

	public void RegisterDialogue(DialogueScript dialogue)
	{
		CurrentDialogue = dialogue;
	}

	public void OnStartState()
	{
		if (IsStarted) return;

		IsStarted = true;

		_currentLine = 0;
		_innerTimer = 0f;
		
		_textData = CSVReader.Read("DialogueKr");
        
		if (CurrentDialogue.fadeOnStart)
			FadeContainer.Instance.FadeTo(1f, 0f, Color.black);
		SpeechContainer.Instance.Show();
		ImageContainer.Instance.HideImage();
		
		// StringBuilder sb = new StringBuilder();
        
		// foreach (var content in data)
		// {
		// 	sb.Append(content.Key);
		// 	foreach (var kv in content.Value)
		// 	{
		// 		sb.Append("\n  ");
		// 		sb.Append(kv.Key);
		// 		sb.Append(" : ");
		// 		sb.Append(kv.Value);
		// 	}
		// 	sb.Append("\n-----------------------\n");
		// }
		//
		// Debug.Log(sb.ToString());
	}

	public void OnState()
	{
		var dt = Time.deltaTime;
		_innerTimer += dt;
		
		//TODO: Implement for events
		if (CurrentDialogue is null)
			return;

		if (_currentLine >= CurrentDialogue.Count)
		{
			Game.Instance.SetState(null);
			OnEndEvent?.Invoke();
			return;
		}
		
		var data = CurrentDialogue.DialogueEvents[_currentLine];

		switch (data.Type)
		{
			case DialogueEventType.None:
				if (data.ImageOff)
					ImageContainer.Instance.HideImage();
				
				if (data.TextboxOff)
					SpeechContainer.Instance.Hide();
				
				if (data.Wait && _innerTimer < data.Duration)
				{
					return;
				}
				else
				{
					_currentLine++;
					_innerTimer = 0f;
				}
				break;
			case DialogueEventType.Speech:
				string rawText = _textData.ContainsKey(data.Text) 
						? _textData[data.Text]["Content"] : data.Text;

				if (_currentParam != null)
					rawText = rawText.Replace("%s", _currentParam);
				
				string rawSpeaker = (data.Speaker.Length == 0) ?
				                    (_textData.ContainsKey(data.Text) 
					                    ? _textData[data.Text]["Speaker"]
					                    : data.Speaker) : data.Speaker;
				
				if (_innerTimer < dt + Constants.Epsilon)
					SpeechContainer.Instance.Show();
				
				if (data.Wait && _innerTimer <= (rawText.Length - 1) * 0.05f * data.TimeMult)
				{
					string outText = rawText.Substring(0, 
						Mathf.Min(rawText.Length, (int)(_innerTimer / (0.05f * data.TimeMult)*2)));
					
					outText = Encoding.Default.GetString(
							Encoding.Default.GetBytes(outText));
					SpeechContainer.Instance.SetText(rawSpeaker, outText);
					
					if (GlobalInputController.Instance.ConfirmPressed || rawText.Length <= outText.Length)
					{
						_innerTimer = rawText.Length * 0.05f * data.TimeMult;
						SpeechContainer.Instance.SetText(rawSpeaker, rawText);
					}
				}
				else if (GlobalInputController.Instance.ConfirmPressed)
				{
					_currentLine++;
					_innerTimer = 0f;
				}
				else
				{
					SpeechContainer.Instance.SetText(rawSpeaker, rawText);
				}
				break;
			case DialogueEventType.Image:
				var targetImage = data.Image;
				
				if (_innerTimer < dt + Constants.Epsilon)
				{
					if (targetImage is not null)
					{
						ImageContainer.Instance.ShowImage();
						ImageContainer.Instance.SetImage(targetImage, data.ImageRect);
					}
				}
				if (data.Wait && _innerTimer < data.Duration)
				{
					if (targetImage is null)
					{
						ImageContainer.Instance.SetAlpha(1f - _innerTimer / data.Duration);
					}
					else
					{
						ImageContainer.Instance.SetAlpha(_innerTimer / data.Duration);
					}
					return;
				}
				else
				{
					if (targetImage is null)
					{
						ImageContainer.Instance.SetImage(null);
						ImageContainer.Instance.SetAlpha(0f);
					}
					else
					{
						ImageContainer.Instance.SetAlpha(1f);
					}
					_currentLine++;
					_innerTimer = 0f;
				}
				break;
			case DialogueEventType.Shake:
				if (!data.Wait)
				{
					Game.Instance.StartCoroutine(ShakeRoutine(data.Power, data.Duration));
					_currentLine++;
					_innerTimer = 0f;
					return;
				}
				
				if (data.Wait && _innerTimer < data.Duration)
				{
					ImageContainer.Instance.Rect.anchoredPosition
						= new Vector2(
							Random.Range(-data.Power, data.Power), 
							Random.Range(-data.Power, data.Power));
					return;
				}
				else
				{
					ImageContainer.Instance.Rect.anchoredPosition = Vector2.zero;
					_currentLine++;
					_innerTimer = 0f;
				}
				break;
			case DialogueEventType.Fade:
				if (_innerTimer < dt + Constants.Epsilon)
					FadeContainer.Instance.FadeTo(data.Destination, data.Duration, data.Color);
				if (!data.Wait || _innerTimer > data.Duration)
				{
					_currentLine++;
					_innerTimer = 0f;
				}
				break;
			case DialogueEventType.Choice:
				string rawChoices = "";

				for (int idx = 0; idx < data.Choices.Count; idx++)
				{
					var choice = data.Choices[idx];
					var marker = ChoiceCache == idx ? "???" : "";
					var rawChoice = _textData.ContainsKey(choice) ? _textData[choice]["Content"].ToString() : choice;
					rawChoices += marker + "\t" + rawChoice + "\n";
				}
				
				if (_innerTimer < dt + Constants.Epsilon)
					SpeechContainer.Instance.Show();
				
				SpeechContainer.Instance.SetText("", rawChoices);
				
				if (_innerTimer < dt + Constants.Epsilon)
				{
					ChoiceCache = 0;
					return;
				}
				
				if (GlobalInputController.Instance.ConfirmPressed)
				{
					_currentLine++;
					_innerTimer = 0f;
				}
				
				if (GlobalInputController.Instance.CurrentFrameAxis.y > 0.5f)
				{
					ChoiceCache = Mathf.Max(0, ChoiceCache - 1);
				}
				else if (GlobalInputController.Instance.CurrentFrameAxis.y < -0.5f)
				{
					ChoiceCache = Mathf.Min(data.Choices.Count - 1, ChoiceCache + 1);
				}
				
				OnChoiceResult?.Invoke(ChoiceCache);
				break;
			case DialogueEventType.Gather:
				string rawAnswers = "";

				for (int idx = 0; idx < data.Choices.Count; idx++)
				{
					var choice = data.Choices[idx];
					var marker = ChoiceCache == idx ? "???" : "";
					var rawChoice = _textData.ContainsKey(choice) ? _textData[choice]["Content"].ToString() : choice;
					
					rawAnswers += marker + "\t" + rawChoice
					              + ((idx == 0) ? "\t" + new string('|', ChoiceStack + 1) 
					                                   + new string('_', 10 - ChoiceStack) + "|\n"
						              : "\n");
				}
				
				if (_innerTimer < dt + Constants.Epsilon)
					SpeechContainer.Instance.Show();
				
				SpeechContainer.Instance.SetText(data.Speaker.Replace("%s", _currentParam), rawAnswers);

				if (_innerTimer < dt + Constants.Epsilon)
				{
					ChoiceCache = 0;
					return;
				}

				if (GlobalInputController.Instance.ConfirmPressed)
				{
					if (ChoiceCache == 0)
					{
						ChoiceStack++;
					}
					
					if (ChoiceStack >= 10 || ChoiceCache > 0)
					{
						_currentLine++;
						_innerTimer = 0f;
						ChoiceStack = 0;
				
						OnChoiceResult?.Invoke(ChoiceCache);
					}
				}

				if (GlobalInputController.Instance.CurrentFrameAxis.y > 0.5f)
				{
					ChoiceCache = Mathf.Max(0, ChoiceCache - 1);
				}
				else if (GlobalInputController.Instance.CurrentFrameAxis.y < -0.5f)
				{
					ChoiceCache = Mathf.Min(data.Choices.Count - 1, ChoiceCache + 1);
				}
				
				break;
			case DialogueEventType.Title:
				if (_innerTimer < dt + Constants.Epsilon)
				{
					TitleContainer.Instance.SetTitle(data.Text, data.Duration, _currentParam);
					return;
				}

				if (_innerTimer < 2f + data.Duration)
				{
					return;
				}
				else
				{
					_currentLine++;
					_innerTimer = 0f;
				}
				break;
			case DialogueEventType.Sound:
				if (_innerTimer < dt + Constants.Epsilon)
					Game.Instance.AudioSource.PlayOneShot(data.Sound);
				
				if (data.Wait && _innerTimer < data.Sound.length)
				{
					return;
				}
				else
				{
					_currentLine++;
					_innerTimer = 0f;
				}
				break;
		}
	}

	IEnumerator ShakeRoutine(float power, float duration)
	{
		var it = 0f;
		while (it < duration)
		{
			var dt = Time.deltaTime;
			it += dt;
			
			ImageContainer.Instance.Rect.anchoredPosition
				= new Vector2(
					Random.Range(-power, power), 
					Random.Range(-power, power));

			yield return null;
		}
		
		ImageContainer.Instance.Rect.anchoredPosition = Vector2.zero;
	}

	public void OnEndState()
	{
		ImageContainer.Instance.HideImage();
		SpeechContainer.Instance.Hide();
		FadeContainer.Instance.FadeTo(0f, 0f, Color.black);
	}
}
