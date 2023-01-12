using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public bool isRevealed;
    public string keyword;

    [SerializeField] TMP_Text textBox;
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CustomAnimator anim;
    [SerializeField] private BoxCollider2D collider;

    [SerializeField] private bool isContacted;
    [SerializeField] private int level;
    
    void Start()
    {
        anim = GetComponent<CustomAnimator>();
        collider = GetComponent<BoxCollider2D>();
    }

    private void LateUpdate()
    {
        var hit = Physics2D.BoxCast(transform.position, collider.size, 0, 
            Vector2.zero, 0, 1 << LayerMask.NameToLayer("Player"));

        if (hit)
        {
            if (!isContacted)
                OnHover();
            isContacted = true;
        }
        else
        {
            if (isContacted)
                OnLeave();
            isContacted = false;
        }
        
        if (GlobalInputController.Instance.ConfirmPressed && isContacted)
        {
            OnInteract();
        }
    }

    void OnHover()
    {
        if (isRevealed) return;

        // TODO: test code
        spriteRenderer.color = Color.red;
    }
    
    void OnLeave()
    {
        if (isRevealed) return;

        // TODO: test code
        spriteRenderer.color = Color.cyan;
    }

    void OnInteract()
    {
        if (isRevealed) return;
        
        level++;

        if (level > 9)
        {
            isRevealed = true;
            spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
        }
    }
}
