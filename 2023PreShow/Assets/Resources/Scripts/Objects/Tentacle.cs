using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    // TODO: Should be changed into custom animator 
    private SpriteRenderer _sr;
    private CustomAnimator _animator;

    private const float Duration = 0.4f;
    private float _innerTimer = 0f;
    

    private void Awake()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<CustomAnimator>();
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
    }

    public void Shake()
    {
        _innerTimer = Duration;
    }

    public void SetAnim(string key)
    {
        _animator.SetAnim(key);
        _animator.SetNextAnim("TentacleIdle");
    }
}
