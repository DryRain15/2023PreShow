using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(DialogueEventData), true)]
public class DialogueEventDataPropertyDrawer : PropertyDrawer
{
	public override float GetPropertyHeight ( SerializedProperty property, GUIContent label )
	{
		switch (property.FindPropertyRelative("_type").enumValueIndex)
		{
			case 1:
				return 96f;
			case 2:
				return 120f;
			case 3:
				return 96f;
			case 4:
				return 120f;
			case 5:
				return 60f + property.FindPropertyRelative("Choices").arraySize * 24f;
			case 6:
				return 60f + property.FindPropertyRelative("Choices").arraySize * 24f;
			case 7:
				return 72f;
			case 8:
				return 72f;
			default:
				return 120f;
		}
	}
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		// Draw the property using the default inspector
		EditorGUI.BeginProperty(position, label, property);

		var m_h = 24f;
		
		var tempRect = new Rect(position.x, position.y, position.width, 20f);
		EditorGUI.PropertyField(tempRect, property.FindPropertyRelative("_type"));
		tempRect.y += m_h;
		
		switch (property.FindPropertyRelative("_type").enumValueIndex)
		{
			case 1:
				EditorGUI.PropertyField(tempRect, property.FindPropertyRelative("Text"));
				tempRect.y += m_h;
				EditorGUI.PropertyField(tempRect, property.FindPropertyRelative("Speaker"));
				tempRect.y += m_h;
				EditorGUI.PropertyField(tempRect, property.FindPropertyRelative("TimeMult"));
				tempRect.y += m_h;
				break;
			case 2:
				EditorGUI.PropertyField(tempRect, property.FindPropertyRelative("Image"));
				tempRect.y += m_h;
				EditorGUI.PropertyField(tempRect, property.FindPropertyRelative("ImageRect"));
				tempRect.y += m_h * 2;
				EditorGUI.PropertyField(tempRect, property.FindPropertyRelative("Duration"));
				tempRect.y += m_h;
				break;
			case 3:
				EditorGUI.PropertyField(tempRect, property.FindPropertyRelative("Wait"));
				tempRect.y += m_h;
				EditorGUI.PropertyField(tempRect, property.FindPropertyRelative("Power"));
				tempRect.y += m_h;
				EditorGUI.PropertyField(tempRect, property.FindPropertyRelative("Duration"));
				tempRect.y += m_h;
				break;
			case 4:
				EditorGUI.PropertyField(tempRect, property.FindPropertyRelative("Wait"));
				tempRect.y += m_h;
				EditorGUI.PropertyField(tempRect, property.FindPropertyRelative("Destination"));
				tempRect.y += m_h;
				EditorGUI.PropertyField(tempRect, property.FindPropertyRelative("Color"));
				tempRect.y += m_h;
				EditorGUI.PropertyField(tempRect, property.FindPropertyRelative("Duration"));
				tempRect.y += m_h;
				break;
			case 5:
				EditorGUI.PropertyField(tempRect, property.FindPropertyRelative("Choices"));
				tempRect.y += m_h * property.FindPropertyRelative("Choices").arraySize;
				break;
			case 6:
				EditorGUI.PropertyField(tempRect, property.FindPropertyRelative("Choices"));
				tempRect.y += m_h * property.FindPropertyRelative("Choices").arraySize;
				break;
			case 7:
				EditorGUI.PropertyField(tempRect, property.FindPropertyRelative("Text"));
				tempRect.y += m_h;
				EditorGUI.PropertyField(tempRect, property.FindPropertyRelative("Duration"));
				tempRect.y += m_h;
				break;
			case 8:
				EditorGUI.PropertyField(tempRect, property.FindPropertyRelative("Wait"));
				tempRect.y += m_h;
				EditorGUI.PropertyField(tempRect, property.FindPropertyRelative("Sound"));
				tempRect.y += m_h;
				break;
			default:
				EditorGUI.PropertyField(tempRect, property.FindPropertyRelative("Wait"));
				tempRect.y += m_h;
				EditorGUI.PropertyField(tempRect, property.FindPropertyRelative("Duration"));
				tempRect.y += m_h;
				EditorGUI.PropertyField(tempRect, property.FindPropertyRelative("TextboxOff"));
				tempRect.y += m_h;
				EditorGUI.PropertyField(tempRect, property.FindPropertyRelative("ImageOff"));
				tempRect.y += m_h;
				break;
		}
		
		EditorGUI.EndProperty();
	}
}