using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomAnimation", menuName = "Custom2D/Create Custom Animation", order = 1)]
public class CustomAnimation : ScriptableObject
{
    [SerializeField]
    private int defaultDuration = 100;

    public List<SpriteFrame> spriteSequence;
    public int Size => spriteSequence.Count;

    public SpriteFrame GetFrame(int frame)
    {
        if (spriteSequence is null || spriteSequence.Count <= frame)
        {
            return new SpriteFrame()
            {
                sprite = ResourceStorage.Instance.emptySprite,
                duration = 100,
            };;
        }
        return spriteSequence[frame];
    }

    public Sprite[] InputBuffer
    {
        set
        {
            spriteSequence ??= new List<SpriteFrame>();
            foreach (var spr in value)
            {
                spriteSequence.Add(new SpriteFrame(spr, defaultDuration));
            }
        }
    }
}

[Serializable]
public struct SpriteFrame
{
    public Sprite sprite;
    
    /// <summary>
    /// frame duration (ms)
    /// </summary>
    public int duration;

    public SpriteFrame(Sprite spr, int t = 100)
    {
        sprite = spr;
        duration = t;
    }
}
