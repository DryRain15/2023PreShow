using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStorage : MonoBehaviour
{
    public static ResourceStorage Instance;
    
    public Material lineMaterial;

    public Sprite emptySprite;

    public DialogueScript prologue;
    public DialogueScript ask;
    public DialogueScript fadeIn;
    public DialogueScript fadeOut;

    public DialogueScript ending;

    public List<AudioClip> clipPools;
    public List<AudioClip> popClips;

    private void Awake()
    {
        Instance = this;
    }
    
    public AudioClip GetRandomClip()
    {
        return clipPools[UnityEngine.Random.Range(0, clipPools.Count)];
    }

    public AudioClip GetRandomPops()
    {
        return popClips[UnityEngine.Random.Range(0, popClips.Count)];
    }
}
