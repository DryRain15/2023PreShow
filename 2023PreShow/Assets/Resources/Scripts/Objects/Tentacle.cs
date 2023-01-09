using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    // TODO: Should be changed into custom animator 
    private SpriteRenderer _sr;

    private const float Duration = 0.4f;
    private float _innerTimer = 0f;
    
    public Color DefaultColor = Color.white;
    public Color ShakeColor = Color.red;
    public Color ScratchColor = Color.cyan;

    private void Awake()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var dt = Time.deltaTime;
        
        if (_innerTimer > 0f)
        {
            _innerTimer -= dt;
        }
        else
        {
            _innerTimer = 0f;
        }
        
        _sr.color = Color.Lerp(DefaultColor, ShakeColor, _innerTimer / Duration);
    }

    public void Shake()
    {
        _innerTimer = Duration;
    }
}
