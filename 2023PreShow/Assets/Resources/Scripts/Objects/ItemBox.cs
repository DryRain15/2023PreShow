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

    private readonly Color HiddenColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    
    void Start()
    {
        anim = GetComponent<CustomAnimator>();
        collider = GetComponent<BoxCollider2D>();
        spriteRenderer.color = Color.cyan;
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
        // TODO: test code
        spriteRenderer.color = Color.white;
    }
    
    void OnLeave()
    {
        // TODO: test code
        spriteRenderer.color = HiddenColor;
    }

    void OnInteract()
    {
        if (isRevealed)
        {
            Game.Instance.YieldState(new YieldForEvent(ResourceStorage.Instance.ask, keyword));
            return;
        }
        
        level++;
        anim.SetFrame(level);

        if (level > 9)
        {
            isRevealed = true;
            OnReveal();
        }
    }

    void OnReveal()
    {
        spriteRenderer.transform.localPosition = new Vector3(0f, 0f, 1f);
        level = 0;
    }
}
