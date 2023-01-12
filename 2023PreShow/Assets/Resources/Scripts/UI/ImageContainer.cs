using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageContainer : MonoBehaviour
{
    public static ImageContainer Instance;

    public GameObject Container;
    public RectTransform Rect;
    
    [SerializeField] private Image image;
    [SerializeField] private GameObject imageObject;

    private float _targetAlpha = 0f;
    private float _prevAlpha = 0f;
    private float _targetDuration = 0f;
    private float _innerTimer = 0f;
    
    private void Awake()
    {
        Instance = this;
        imageObject = image.gameObject;
        Container = gameObject;
        Rect = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        HideImage();
    }

    // Update is called once per frame
    void Update()
    {
        var dt = Time.deltaTime;

        if (_targetDuration >= 0f)
        {
            _innerTimer += dt;
            _innerTimer = Mathf.Clamp(_innerTimer, 0f, _targetDuration);

            var c = image.color;
            image.color = new Color(1f, 1f, 1f, 
                Mathf.Lerp(_prevAlpha, _targetAlpha, _innerTimer / _targetDuration));
            
            if (_innerTimer >= _targetDuration)
            {
                image.color = new Color(1f, 1f, 1f,_targetAlpha);
                _targetDuration = -1f;
                _innerTimer = 0f;
            }
        }
    }
    
    public void SetImage(Sprite sprite, Rect rect)
    {
        image.sprite = sprite;
        imageObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(rect.x, rect.y);
        imageObject.GetComponent<RectTransform>().sizeDelta = new Vector2(rect.width, rect.height);
    }
    
    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
    }
    
    public void ShowImage()
    {
        imageObject.SetActive(true);
    }
    
    public void HideImage()
    {
        imageObject.SetActive(false);
    }
    
    public void FadeTo(float alpha, float duration)
    {
        _targetAlpha = alpha;
        _targetDuration = duration;
        _prevAlpha = image.color.a;
        _innerTimer = 0f;
    }
    
    public void SetAlpha(float alpha)
    {
        _targetAlpha = alpha;
        image.color = new Color(1f, 1f, 1f,_targetAlpha);
    }
}
