using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechContainer : MonoBehaviour
{
    public static SpeechContainer Instance;
    
    [SerializeField] private TMP_Text mSpeech;
    [SerializeField] private TMP_Text mSpeaker;

    [SerializeField] private Image speechBox;
    [SerializeField] private Image speakerBox;
    
    [SerializeField] private GameObject speechContainer;
    
    private void Awake()
    {
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string speaker, string speech)
    {
        this.mSpeaker.text = speaker;
        this.mSpeech.text = speech;
    }
    
    public void SetText(string speech)
    {
        this.mSpeech.text = speech;
    }
    
    public void SetSpeaker(string speaker)
    {
        this.mSpeaker.text = speaker;
    }
    
    // TODO: Transition needed
    public void Show()
    {
        speechContainer.SetActive(true);
    }
    
    // TODO: Transition needed
    public void Hide()
    {
        SetText("", "");
        speechContainer.SetActive(false);
    }
}
