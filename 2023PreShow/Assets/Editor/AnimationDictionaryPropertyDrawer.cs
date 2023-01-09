﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AnimationDictionary))]
[CustomPropertyDrawer(typeof(AxisKeyDictionary))]
[CustomPropertyDrawer(typeof(AdjacentPathDictionary))]
public class AnimationDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer
{
}
