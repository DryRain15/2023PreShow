using System;
using System.Collections;
using System.Collections.Generic;
using Proto.BasicExtensionUtils;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance;
    public Transform target;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!target)
            return;
        
        var dt = Time.deltaTime;
        var targetPos = target.position;
        var pos = transform.position.ToVector2();
        pos = Vector2.Lerp(pos, targetPos, dt * 5);
        transform.position = pos.ToVector3(-10f);
        
    }
}
