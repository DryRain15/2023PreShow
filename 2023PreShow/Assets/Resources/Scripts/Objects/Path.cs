using System;
using System.Collections;
using System.Collections.Generic;
using Proto.BasicExtensionUtils;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class AdjacentPathDictionary : SerializableDictionary<int, float>{}

[Serializable]
public class Path
{
    public int id;
    
    public Vector2 startPoint;
    public Vector2 endPoint;

    public Path Parent { get; set; }
    public int parentIdx;
    public float portion;
    public Vector2 direction;
    public float distance => direction.magnitude;
    
    public AdjacentPathDictionary adjacentPaths = new AdjacentPathDictionary();
    
    public Path(Vector2 startPoint, Vector2 endPoint)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
    }

    public void SetPath(Vector2 startPoint, Vector2 endPoint)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
    }

    public Vector2 GetPoint(float p)
    {
        return startPoint + direction * p;
    }

    public void Validate()
    {
        if (id == 0)
            parentIdx = -1;
        
        if (parentIdx < 0)
        {
            direction = endPoint - startPoint;
            return;
        }

        if (Parent is null)
            return;
        
        startPoint = Parent.direction * portion + Parent.startPoint;
        endPoint = startPoint + direction;
    }
    
    public void DrawGizmos()
    {
        Gizmos.color = Color.white;
        Vector2 leftWidth = direction.normalized.GetLeftPerpendicular() * 0.1f;
        Vector2 rightWidth = direction.normalized.GetRightPerpendicular() * 0.1f;
        Gizmos.DrawLine(startPoint + leftWidth, endPoint + leftWidth);
        Gizmos.DrawLine(startPoint + rightWidth, endPoint + rightWidth);
        
        Gizmos.DrawSphere(startPoint, 0.15f);
        Gizmos.DrawSphere(endPoint, 0.15f);
    }
}

[Serializable]
public class Contact
{
    private Path _self;
    private Path _other;
    public Path Other => _other;
    public float portion;
    public Vector2 direction;

    public Vector2 StartPoint { get; private set; }
    public Vector2 EndPoint { get; private set; }

    public Contact(Path self, float portion, Vector2 direction)
    {
        this._self = self;
        this.portion = portion;
        this.direction = direction;

        StartPoint = self.startPoint + (self.direction * portion);
        EndPoint = StartPoint + direction;
        this._other = new Path(StartPoint, EndPoint);
    }

    public void Validate(Path self)
    {
        if (_self is null || _other is null)
        {
            this._self ??= self;
            StartPoint = self.startPoint + (self.direction * portion);
            EndPoint = StartPoint + direction;
            this._other ??= new Path(StartPoint, EndPoint);
        }
        
        Other?.SetPath(StartPoint, EndPoint);
    }
}