using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeContainer : MonoBehaviour
{
    public static FadeContainer Instance;
    
    [SerializeField] private RawImage image;
    private Color _targetColor = Color.black;
    private float _targetAlpha = 0f;
    private float _prevAlpha = 0f;
    private float _targetDuration = 0f;
    private float _innerTimer = 0f;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var dt = Time.deltaTime;

        if (_targetDuration >= 0f)
        {
            _innerTimer += dt;
            _innerTimer = Mathf.Clamp(_innerTimer, 0f, _targetDuration);

            var c = image.color;
            image.color = new Color(c.r, c.g, c.b, 
                Mathf.Lerp(_prevAlpha, _targetAlpha, _innerTimer / _targetDuration));
            
            if (_innerTimer >= _targetDuration)
            {
                image.color = new Color(c.r, c.g, c.b,_targetAlpha);
                _targetDuration = -1f;
                _innerTimer = 0f;
            }
        }
        
    }
    
    public void FadeTo(float alpha, float duration)
    {
        FadeTo(alpha, duration, Color.black);
    }
    
    public void FadeTo(float alpha, float duration, Color color)
    {
        _targetAlpha = alpha;
        _targetDuration = duration;
        _targetColor = color;
        _prevAlpha = image.color.a;
        _innerTimer = 0f;
    }
}
