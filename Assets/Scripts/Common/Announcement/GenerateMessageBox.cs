using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GenerateMessageBox : MonoBehaviour
{
    public static GenerateMessageBox instance;
    public GameObject prefab;
    public GameObject parent;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
       
    }
    /*
        title : title of popup
        msg: main message of popup
        okText: text of the OK button
        cancelText: text of the Cancel Button
        yesEvent: UnityAction of OK Button
        cancelEvent: UnityAction of Cancel Button
     */
    public void ShowMessageBox(string title, string msg, string okText, string cancelText, UnityAction yesEvent, UnityAction cancelEvent)
    {
        GameObject messageBox = Instantiate(prefab, parent.transform);

        Text titleMes = messageBox.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>();
        Text desMes = messageBox.transform.GetChild(0).GetChild(3).gameObject.GetComponent<Text>();

        Transform listBtns = messageBox.transform.GetChild(0).GetChild(4);

        Button btnYes = listBtns.GetChild(0).GetComponent<Button>();
        Button btnCancel = listBtns.GetChild(1).GetComponent<Button>();

        Text textYes = listBtns.GetChild(0).GetChild(0).GetComponent<Text>();
        Text textCancel = listBtns.GetChild(1).GetChild(0).GetComponent<Text>();

        titleMes.text = title;
        desMes.text = msg;
        textYes.text = okText;
        textCancel.text = cancelText;

        if(yesEvent != null)
        {
            btnYes.onClick.AddListener(yesEvent);
        }

        //destroy message box after click button OK
        btnYes.onClick.AddListener(() =>
        {
            Destroy(messageBox);
        });

        if (cancelEvent != null)
        {
            btnCancel.onClick.AddListener(cancelEvent);
        }

        //destroy message box after click button Cancel
        btnCancel.onClick.AddListener(() =>
        {
            Destroy(messageBox);
        });

        if (cancelText.Equals(""))
        {
            Destroy(listBtns.GetChild(1).gameObject);
        }
    }
}
