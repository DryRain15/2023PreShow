using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AnimationDictionary))]
[CustomPropertyDrawer(typeof(AxisKeyDictionary))]
[CustomPropertyDrawer(typeof(AdjacentPathDictionary))]
[CustomPropertyDrawer(typeof(Tentacles))]
public class AnimationDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer
{
}
