using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class HistoryLog 
{
    public DateTime time;
    public string userName;
    public string userId;
    public NhiVR.Event e;
    public int count;
    public string message;
    public string statecode;
    public string verify;

    public HistoryLog() { }

    public HistoryLog(DateTime time, string userName, NhiVR.Event e, string statecode)
    {
        this.time = time;
        this.userName = userName;
        this.statecode = statecode;
        this.e = e;
        count = 1;
        //set message
        if (e.parameters.typeId == 3 || e.eventcode == "E4" || e.eventcode == "E3")
        {
            this.message = e.eventname;
        } else
        {
            if(e.eventcode.Contains("-T"))
            {
                this.message = e.eventname + " trong " + e.parameters.value + " phút";
            } else
            {
                this.message = e.eventname + " " + e.parameters.value + " "; 
                string unit = DrugEventReference.units[e.eventcode.Replace("-L", "")];
                if(unit != null)
                {
                    this.message += unit;
                }
            }
        }
    }
}
