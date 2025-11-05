using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NhiVR
{
    [Serializable]
    public class Event
    {
        public long eventid;
        public string eventname;
        public Parameter parameters;
        public long methodid;
        public string methodname;
        public string eventcode;
        public int actorNum;
        public float delaySend = 0;
        public string objectname = "";

        public Event(long eventid, string eventname, long methodid, string methodname, string eventcode)
        {
            this.eventid = eventid;
            this.methodid = methodid;
            this.eventname = eventname;
            this.methodname = methodname;
            this.eventcode = eventcode;

        }

        //Execute an event
        public void Execute()
        {
            if (eventcode == "")
            {
                return;
            }

            if (GameStateController.instance.roomstate == 1)
            {
                //set actor of event
                actorNum = PhotonNetwork.LocalPlayer.ActorNumber;

                //Add and synchronize current Event to history log 
                //if (!eventcode.Equals("E4"))
                //{
                //    HistoryLogSynchronize.instance.SynchronizeHistoryLog(new HistoryLog(DateTime.Now, Common.instance.user.userName, this, LessonDataSynchronize.instance.currentState.code), true);
                //}


                ////If current event is a normal injection event
                //if (LessonDataSynchronize.instance.listDrugNormalInjection.ContainsKey(eventcode))
                //{
                //    //Play animation of normal injection event
                //    GameObject.Find(objectname).GetComponent<NormalInjectionModelController>().ActivateAnimation();

                //}
                //else if (LessonDataSynchronize.instance.listDrugMaskAir.ContainsKey(eventcode))
                //{
                //    //play animation of normal air drug event
                //    MaskOxyModelController.instance.ActivateAnimation();
                //    //announce
                //    TextAnnounce.instance.ShowTextAnnounce("Đã thực hiện " + eventname + " " + parameters.value + " " + DrugEventReference.units[eventcode]);
                //}
                //else if (LessonDataSynchronize.instance.listDrugInjectionTiemBap.ContainsKey(eventcode))
                //{
                //    //play animation of tiem bap event
                //    GameObject.Find(objectname).GetComponent<NormalInjectionModelController>().ActivateAnimationTiemBap();
                //}
                //else if (GameObject.Find(eventcode)) //if current event is a cure tool
                //{
                //    //play animation of this cure tool
                //    GameObject tool = GameObject.Find(eventcode);
                //    if (tool.GetComponent<CureToolModelController>())
                //    {
                //        tool.GetComponent<CureToolModelController>().ActivateAnimation();
                //    }
                //}
                //else if (eventcode.Equals("E32"))
                //{
                //    DoctorController.instance.VoLung();
                //    this.delaySend = 3;
                //}
                //else if (eventcode.Equals("E33"))
                //{
                //    DoctorController.instance.AnNguc();
                //    this.delaySend = 0;
                //}

                //Send this event to other players
                EventDataSynchronize.instance.SendEvent(this);

            }
            else
            {
                MessageBoxController.instance.ShowMessageBox("Thông báo", "Lớp học chưa bắt đầu.", "Ok", "", null, null);
            }

        }
    }
}


[Serializable]
public class EventCommand
{
    //an object store data to synchronize animation
    public string eventcode;
    public int actorNum;
    public string objectname;
    public string eventname;
    public float eventvalue;

    public EventCommand(string eventcode, int actorNum, string objectname, string eventname, float eventvalue)
    {
        this.eventcode = eventcode;
        this.actorNum = actorNum;
        this.objectname = objectname;
        this.eventname = eventname;
        this.eventvalue = eventvalue;
    }
}

[Serializable]
public class EventResponeMessage
{
    //an object store data for response message
    public string message;
    public int actorNum;

    public EventResponeMessage(string message, int actorNum)
    {
        this.message = message;
        this.actorNum = actorNum;
    }
}