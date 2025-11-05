using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class MessageBoxController : MonoBehaviour
{

    public Text msg;
    public Text title;
    public Button yesButton;
    public Button cancelButton;
    public GameObject messagePanelObject;

    public static MessageBoxController instance;
    
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    // Yes/No/Cancel: A string, a Yes event, a No event and Cancel event
    public void ShowMessageBox(string title, string msg, string okText, string cancelText, UnityAction yesEvent, UnityAction cancelEvent)
    {
       
        messagePanelObject.SetActive(true);

        this.title.text = title;
        this.msg.text = msg;

        yesButton.onClick.RemoveAllListeners();
        if (yesEvent != null)
            yesButton.onClick.AddListener(yesEvent);
        yesButton.onClick.AddListener(ClosePanel);

        cancelButton.onClick.RemoveAllListeners();
        if (cancelEvent != null)
            cancelButton.onClick.AddListener(cancelEvent);
        cancelButton.onClick.AddListener(ClosePanel);
        yesButton.gameObject.SetActive(true);
        yesButton.transform.GetChild(0).GetComponent<Text>().text = okText;
        if (cancelText.Length > 0)
        {
            cancelButton.gameObject.SetActive(true);
            cancelButton.transform.GetChild(0).GetComponent<Text>().text = cancelText;
        }
        else
            cancelButton.gameObject.SetActive(false);
    }

    public void ClosePanel()
    {
        if (messagePanelObject.activeSelf)
            messagePanelObject.SetActive(false);
    }
   
   
}

