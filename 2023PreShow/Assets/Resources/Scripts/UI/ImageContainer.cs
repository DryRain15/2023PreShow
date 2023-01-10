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
}
