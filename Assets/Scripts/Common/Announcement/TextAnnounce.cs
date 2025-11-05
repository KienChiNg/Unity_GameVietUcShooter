using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextAnnounce : MonoBehaviour
{
    public static TextAnnounce instance;
    public TMP_Text content;
    Queue<string> announces;
    public AudioSource audioSrc;
    private void Awake()
    {
        instance = this;
        announces = new Queue<string>();
    }

    private void Start()
    {
        content.gameObject.GetComponent<CanvasRenderer>().SetAlpha(0);
    }

    public void Update()
    {
        //display announce one by one from queue
        if(content.gameObject.GetComponent<CanvasRenderer>().GetAlpha() == 0 && announces.Count > 0)
        {
            content.text = announces.Dequeue();
            content.gameObject.GetComponent<CanvasRenderer>().SetAlpha(1);
            content.CrossFadeAlpha(0, 6f, false);

            if(audioSrc)
            {
                audioSrc.Play();
            }
        }
    }

    //add announce text to a queue
    public void ShowTextAnnounce(string text)
    {
        announces.Enqueue(text);
    }
}
