using System;
using System.Collections;
using System.Collections.Generic;
using Proto.BasicExtensionUtils;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

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
    
    private LineRenderer _lineRenderer;
    
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

    public void Initiate()
    {
        if (!_lineRenderer)
        {
            Validate();
            
            var go = new GameObject($"Path_{id}");
            go.transform.SetParent(Game.Instance.Storage);
            _lineRenderer = go.AddComponent<LineRenderer>();
            
            _lineRenderer.positionCount = 2;
            _lineRenderer.startColor = Color.white;
            _lineRenderer.endColor = Color.white;
            _lineRenderer.startWidth = 0.2f;
            _lineRenderer.endWidth = 0.2f;
            _lineRenderer.material = ResourceStorage.Instance.lineMaterial;
            _lineRenderer.SetPosition(0, startPoint);
            _lineRenderer.SetPosition(1, endPoint);
        }
    }

    public void Eliminate()
    {
        if (_lineRenderer)
        {
            GameObject.Destroy(_lineRenderer.gameObject);
            _lineRenderer = null;
        }
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