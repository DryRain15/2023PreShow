using System.Collections;
using System.Collections.Generic;
using Proto.Utils;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomAnimation", menuName = "Custom2D/Create Custom Animation Set", order = 1)]
public class CustomAnimationSet : ScriptableObject
{
    [SerializeField] private string defaultName;
    
    public AnimationDictionary animMap;

    public CustomAnimation[] InputBuffer
    {
        set
        {
            animMap ??= new AnimationDictionary();
            foreach (var anim in value)
            {
                animMap.Add(Utils.BuildString(defaultName, ".", anim.name), anim);
            }
        }
    }
}
