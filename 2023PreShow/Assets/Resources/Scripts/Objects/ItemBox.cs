using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public bool isRevealed;
    public string keyword;

    private CustomAnimator anim;
    
    void Start()
    {
        anim = GetComponent<CustomAnimator>();
    }

    void OnHover()
    {
        
    }

    void OnInteract()
    {
        
    }
}
