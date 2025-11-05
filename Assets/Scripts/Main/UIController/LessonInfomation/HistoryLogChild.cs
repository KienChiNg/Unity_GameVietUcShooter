using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryLogChild : MonoBehaviour
{
    public HistoryLog log;
    public Text time;
    public Text user;
    public Text message;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setHistoryLog(HistoryLog log)
    {
        this.log = log;
    }

    public void UpdateTextValue()
    {
        TimeSpan timeSpan = log.time.TimeOfDay;
        time.text = timeSpan.ToString("hh\\:mm\\:ss");
        user.text = log.userName;
        if(log.e.eventcode == "E5" || log.e.eventcode == "E4" || log.e.eventcode == "E3")
        {
            message.text = log.message + " " + log.count + " lần";
        } else
        {
            message.text = log.message;
        }
        
    }
}
