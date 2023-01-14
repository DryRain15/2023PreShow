using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomAnimation))]
public class CustomAnimationEditor : Editor
{
    Object[] DropAreaGUI()
    {
        var evt = Event.current;
        var dropArea = GUILayoutUtility.GetRect(0.0f, 30.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drop sprites here to add");

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
            var anim = (CustomAnimation) target;
            Undo.RecordObject(anim, "Added sprites to Custom Animation via Drag and Drop");
            var sprites = drops.Select(x => (x as Sprite)).ToArray();

            anim.InputBuffer = sprites;
            
            AssetDatabase.SaveAssets();
        }
        
        DrawDefaultInspector();

        var dropArea = GUILayoutUtility.GetRect(0.0f, 20.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, $"Total Duration : {TotalDuration}");
        
    }

    private int TotalDuration
    {
        get
        {
            var anim = (CustomAnimation) target;

            if (anim.spriteSequence is null || anim.spriteSequence.Count == 0)
                return 0;
            return anim.spriteSequence.Sum(seq => seq.duration);
        }
    }
}
