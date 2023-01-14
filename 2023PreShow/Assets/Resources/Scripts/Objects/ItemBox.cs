using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

    private int _halt;

    private readonly Color HiddenColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    
    void Start()
    {
        anim = spriteRenderer.gameObject.GetComponent<CustomAnimator>();
        collider = GetComponent<BoxCollider2D>();
        spriteRenderer.color = Color.cyan;
        anim.ChangeAnimationSpeed(0f);
        anim.SetFrame(level);
    }

    private void OnEnable()
    {
        textBox.text = keyword;
    }

    private void Update()
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
        Game.Instance.PlayClip(ResourceStorage.Instance.GetRandomPops());
    }
    
    void OnLeave()
    {
        // TODO: test code
        spriteRenderer.color = HiddenColor;
        Game.Instance.PlayClip(ResourceStorage.Instance.GetRandomPops());
    }

    void OnInteract()
    {
        if (_halt == 1) return;
        if (_halt == -1)
        {
            _halt = 0;
            return;
        }
        
        Game.Instance.PlayClip(ResourceStorage.Instance.GetRandomClip());
        
        if (isRevealed)
        {
            _halt = 1;
            Game.Instance.YieldState(new YieldForEvent(ResourceStorage.Instance.ask, keyword, OnChoice));
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

    void OnChoice(int choice)
    {
        Game.Instance.SetState(null);
        
        if (choice == 1)
        {
            _halt = -1;
            Game.Instance.PlayClip(ResourceStorage.Instance.GetRandomClip());
            return;
        }
        Game.Instance.SetState(new YieldForEvent(ResourceStorage.Instance.fadeOut));

        var boxName = transform.parent.gameObject.name;
        if (boxName == "Stage1")
        {
            Game.Word1 = keyword;
            Game.Instance.ReserveState(new Stage2Play());
        }
        if (boxName == "Stage2")
        {
            Game.Word2 = keyword;
            Game.Instance.ReserveState(new Stage3Play());
        }
        if (boxName == "Stage3")
        {
            Game.Word3 = keyword;
            // Complete Keywords and glitch ending
            Game.Instance.ReserveState(new YieldForEvent(ResourceStorage.Instance.ending,
                Game.CompleteSentence)
            {
                OnEndEvent = Application.Quit
            });

            if (!Directory.Exists(Application.streamingAssetsPath))
                Directory.CreateDirectory(Application.streamingAssetsPath);

            var path = Application.streamingAssetsPath + "/keywords.txt";
            StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
            
            Game.Instance.CompleteText.text = "";

            writer.WriteLine(Game.CompleteSentence);
            
            writer.Close();
        }
        
        Game.Instance.CompleteText.text = Game.CompleteSentence;
    }
}
