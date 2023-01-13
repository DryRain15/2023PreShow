using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStorage : MonoBehaviour
{
    public static ResourceStorage Instance;
    
    public Material lineMaterial;

    public DialogueScript prologue;
    public DialogueScript ask;

    private void Awake()
    {
        Instance = this;
    }
}
