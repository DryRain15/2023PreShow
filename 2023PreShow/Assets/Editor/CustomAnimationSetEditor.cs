using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomAnimationSet))]
public class CustomAnimationSetEditor : Editor
{
    
    Object[] DropAreaGUI()
    {
        var evt = Event.current;
        var dropArea = GUILayoutUtility.GetRect(0.0f, 30.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drop animations here to add");

        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dropArea.Contains(evt.mousePosition))
                    return null;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    return DragAndDrop.objectReferences;
                }
                break;
        }
        return null;
    }

    public override void OnInspectorGUI()
    {
        var drops = DropAreaGUI();
        if (drops != null)
        {
            //the following line gives me error
            var anims = drops.Select(x => (x as CustomAnimation)).ToArray();

            var animSet = (CustomAnimationSet) target;
            animSet.InputBuffer = anims;
        }

        DrawDefaultInspector();
    }
}
