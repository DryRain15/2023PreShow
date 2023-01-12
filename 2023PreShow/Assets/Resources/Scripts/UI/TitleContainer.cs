using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleContainer : MonoBehaviour
{
    public static TitleContainer Instance;

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
        text = GetComponentInChildren<TMP_Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        var dt = Time.deltaTime;

        if (_targetDuration >= 0f)
        {
            _innerTimer += dt;
            _innerTimer = Mathf.Clamp(_innerTimer, 0f, _introTime + _targetDuration + _outroTime);

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
                SetAlpha((_innerTimer - (_targetDuration + _introTime)) / _outroTime);
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
    
    public void SetTitle(string txt, float intro = 0.7f, float outro = 1.0f, float duration = 3f)
    {
        Show();
        text.text = txt;
        _introTime = intro;
        _outroTime = outro;
        _targetDuration = duration;
        _innerTimer = 0f;
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
