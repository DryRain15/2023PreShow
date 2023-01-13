using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proto.BasicExtensionUtils;

[Serializable]
public class AnimationDictionary : SerializableDictionary<string, CustomAnimation>{}

public class CustomAnimator : MonoBehaviour
{
    public SpriteRenderer sr;
    public CustomAnimation clip;

    public string animName;
    public CustomAnimationSet animSet;
    
    public string nextAnimName;

    public int frameIndex = 0;
    public float animSpeed = 1f;

    public bool isLoop = false;

    private float _innerTimer;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (animSpeed == 0f)
        {
            return;
        }

        var currentFrame = clip.GetFrame(frameIndex);

        if (_innerTimer * 1000 > currentFrame.duration)
        {
            if (animSpeed > 0f)
                frameIndex++;
            else
                frameIndex--;

            _innerTimer = 0f;
            
            if (frameIndex >= clip.Size || frameIndex < 0)
            {
                if (isLoop)
                    frameIndex *= clip.Size * -1;
                else if (nextAnimName is not null)
                {
                    SetAnim(nextAnimName, true);
                    nextAnimName = null;
                }
                
                frameIndex = frameIndex.Clamp(clip.Size - 1);
            }
            sr.sprite = currentFrame.sprite;
        }


        _innerTimer += Time.deltaTime * animSpeed;
    }

    public void ChangeAnimationSpeed(float value)
    {
        animSpeed = value;
        
        if (animSpeed == 0f)
        {
            _innerTimer = 0f;
        }
    }

    public void SetAnim(string key, bool loop = false)
    {
        isLoop = loop;
        if (key == animName)
            return;
        if (!animSet.animMap.ContainsKey(key)) return;
        animName = key;
        clip = animSet.animMap[key];
        frameIndex = 0;
        sr.sprite = clip.GetFrame(frameIndex).sprite;
    }

    public void SetNextAnim(string key)
    {
        isLoop = false;
        if (key == animName)
            return;
        if (!animSet.animMap.ContainsKey(key)) return;
        nextAnimName = key;
    }

    public void SetSpriteAlpha(float value)
    {
        var c = sr.color;
        sr.color = new Color(c.r, c.g, c.b, value);
    }
    public void SetSpriteColor(Color c)
    {
        var a = sr.color.a;
        sr.color = new Color(c.r, c.g, c.b, a);
    }
}