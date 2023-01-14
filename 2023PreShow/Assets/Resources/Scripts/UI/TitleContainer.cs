using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleContainer : MonoBehaviour
{
    public static TitleContainer Instance;
    private Dictionary<string, Dictionary<string, string>> _textData;

    [SerializeField] private TMP_Text text;

    private float _targetAlpha = 0f;
    private float _prevAlpha = 0f;
    private float _introTime = 0f;
    private float _outroTime = 0f;
    private float _targetDuration = 0f;
    private float _innerTimer = 0f;
    
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Hide();
        _textData = CSVReader.Read("DialogueKr");
    }

    // Update is called once per frame
    void Update()
    {
        var dt = Time.deltaTime;

        if (_targetDuration >= 0f)
        {
            _innerTimer += dt;

            if (_innerTimer < _introTime)
            {
                SetAlpha(_innerTimer / _introTime);
            }
            else if (_innerTimer < _introTime + _targetDuration)
            {
                SetAlpha(1f);
            }
            else if (_innerTimer < _introTime + _targetDuration + _outroTime)
            {
                SetAlpha(1f - (_innerTimer - (_targetDuration + _introTime)) / _outroTime);
            }
            else
            {
                SetAlpha(0f);
                _introTime = -1f;
                _outroTime = -1f;
                _targetDuration = -1f;
                _innerTimer = 0f;

                Hide();
            }
        }
    }
    
    public void SetTitle(string txt, float intro, float outro, float duration = 3f, string param = "")
    {
        Show();
        text.text = (_textData.ContainsKey(txt)
            ? _textData[txt]["Content"]
            : txt).Replace("%s", param);
        _introTime = intro;
        _outroTime = outro;
        _targetDuration = duration;
        _innerTimer = 0f;
    }
    
    public void SetTitle(string txt, float duration = 3f, string param = "")
    {
        SetTitle(txt, 0.7f, 1.0f, duration, param);
    }
    
    public void Show()
    {
        text.gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        text.gameObject.SetActive(false);
    }
    
    public void SetAlpha(float alpha)
    {
        text.color = new Color(1f, 1f, 1f,alpha);
    }
}
