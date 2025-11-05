using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System;

public class EventDataSynchronize : MonoBehaviour, IOnEventCallback
{
    public static EventDataSynchronize instance;
    public bool allowSend;
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        allowSend = false;
    }

    // Send event to SERVER or MASTER CLIENT
    public void SendEvent(NhiVR.Event action)
    {
        //if Master client call SEND EVENT, call APISENDEVENT to send this event to SERVER
        //else SEND this event to MASTER CLIENT
        if (!PhotonNetwork.IsMasterClient)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(PhotonEventCodes.EVENT_DATA, ConvertData.ObjectToByteArray(action), raiseEventOptions, sendOptions);
        } else
        {
            //Call API to send event
            StartCoroutine(APISendEvent(action, action.delaySend));

            EventCommand command = new EventCommand(action.eventcode, action.actorNum, action.objectname, action.eventname, action.parameters.value);
            //send command to all client to synchronize animation
            SynchronizeEventAnimation(command);
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            //call when Other players send EVENT to MASTER CLIENT 
            case PhotonEventCodes.EVENT_DATA:

                NhiVR.Event action = (NhiVR.Event)ConvertData.ByteArrayToObject((byte[])photonEvent.CustomData);
                if(PhotonNetwork.IsMasterClient)
                {
                    //Call API to send event
                    StartCoroutine(APISendEvent(action, action.delaySend));

                    EventCommand command = new EventCommand(action.eventcode, action.actorNum, action.objectname, action.eventname, action.parameters.value);
                    //Synchronize animation with command
                    EventCommandHandle(command);

                    //send command to all client to synchronize animation
                    SynchronizeEventAnimation(command);
                }
                break;

            //call when master client synchronize animation
            case PhotonEventCodes.SEND_EVENT_COMMAND:
                
                //all player EXCEPT MASTER CLIENT and PLAYER WHO EXECUTE EVENT synchronize event animation
                if(!PhotonNetwork.IsMasterClient)
                {
                    EventCommand command = (EventCommand)ConvertData.ByteArrayToObject((byte[])photonEvent.CustomData);

                    if(PhotonNetwork.LocalPlayer.ActorNumber != command.actorNum)
                    {
                        EventCommandHandle(command);
                    }
                }
                break;

            //call when synchonize respone message
            case PhotonEventCodes.EVENT_RESPONSE_MESSAGE:

                EventResponeMessage mesRes = (EventResponeMessage)ConvertData.ByteArrayToObject((byte[])photonEvent.CustomData);
                GenerateMessageBox.instance.ShowMessageBox("Thông báo", mesRes.message, "Ok", "", null, null);
                break;
        }
    }

    //Synchronize event animation
    public void SynchronizeEventAnimation(EventCommand command)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEventCodes.SEND_EVENT_COMMAND, ConvertData.ObjectToByteArray(command), raiseEventOptions, sendOptions);
    }

    //
    public void EventCommandHandle(EventCommand command)
    {
        ////Call ActivateOtherPlayerAnimation() function to run Animation in Others' scene
        //if (LessonDataSynchronize.instance.listDrugNormalInjection.ContainsKey(command.eventcode))
        //{
        //    //Play animation of normal injection event
        //    GameObject.Find(command.objectname).GetComponent<NormalInjectionModelController>().ActivateOtherPlayerAnimation();
        //} else if (LessonDataSynchronize.instance.listDrugMaskAir.ContainsKey(command.eventcode))
        //{
        //    MaskOxyModelController.instance.ActivateOtherPlayerAnimation();
        //    //announce
        //    TextAnnounce.instance.ShowTextAnnounce("Đã thực hiện " + command.eventname + " " + command.eventvalue + " " + DrugEventReference.units[command.eventcode]);
        //}
        //else if (LessonDataSynchronize.instance.listDrugInjectionTiemBap.ContainsKey(command.eventcode))
        //{
        //    //play animation of tiem bap event
        //    GameObject.Find(command.objectname).GetComponent<NormalInjectionModelController>().ActivateAnimationTiemBap();
        //}
        //else if (GameObject.Find(command.eventcode))
        //{
        //    GameObject tool = GameObject.Find(command.eventcode);
        //    if (tool.GetComponent<CureToolModelController>())
        //    {
        //        tool.GetComponent<CureToolModelController>().ActivateOtherPlayerAnimation();
        //    }
        //}
        //else if (command.eventcode.Equals("E32"))
        //{
        //    DoctorController.instance.VoLung();
        //    //this.delaySend = 5;
        //}
        //else if (command.eventcode.Equals("E33"))
        //{
        //    DoctorController.instance.AnNguc();
        //    //this.delaySend = 5;
        //}

    }

    //send event with API
    public IEnumerator APISendEvent(NhiVR.Event action, float delaySend)
    {
        yield return new WaitForSeconds(delaySend);

        if(allowSend)
        {
            JSONObject form = new JSONObject();
            form.AddField("room_id", Room.instance.roomId);
            form.AddField("timecreated", DateTime.Now.Ticks);

            JSONObject newEvent = new JSONObject();
            newEvent.AddField("eventcode", action.eventcode);
            newEvent.AddField("methodid", action.methodid);
            newEvent.AddField("typeid", action.parameters.typeId);

            switch (action.parameters.typeId)
            {
                case TypeParameterCode.FLOAT_DON:
                    newEvent.AddField("value", action.parameters.value);
                    break;

                case TypeParameterCode.STRING:
                    newEvent.AddField("value", action.parameters.value_str);
                    break;

                case TypeParameterCode.SO_LAN:
                    newEvent.AddField("value", action.parameters.value + 1);
                    action.parameters.value = 0;
                    break;
                case TypeParameterCode.FLOAT_DAI:
                    newEvent.AddField("value", action.parameters.value);
                    break;
            }

            form.AddField("event", newEvent);
            StartCoroutine(APIController.instance.PostData(gameObject, "OnSendEventFinish", "/lesson/sendevent", form.ToString(), false, 30f, action.actorNum));
        }
    }

    //On Send Event API finish
    public void OnSendEventFinish(JSONObject data)
    {
        JSONObject body = new JSONObject(data.GetField("body").str);
        string eventcode = body.GetField("event").GetField("eventcode").str;
        int[] tar = { (int)data.GetField("actornum").i };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = tar };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        //
        if (data.GetField("text").str == "")
        {
            Debug.Log("Lỗi kết nối");
        } else
        {
            JSONObject j = new JSONObject(data.GetField("text").str);
            if(j.GetField("code").i != 200)
            {
          
                        //create announce
                        EventResponeMessage mes = new EventResponeMessage("Hành động không chính xác!!!", (int)data.GetField("actornum").i);
                        //
                        if (eventcode == "E3" || eventcode == "E4" || eventcode == "E5")
                        {
                            return;
                        }
                        
                        //check event is correct
                        //List<LESSON_DETAILS> lesson_details = LessonDataSynchronize.instance.lesson_details.FindAll((x) => x.current_state.Equals(LessonDataSynchronize.instance.currentState.code));

                        //for (int i = 0; i < lesson_details.Count; i++)
                        //{
                        //    if (lesson_details[i].required_events.Contains(eventcode))
                        //    {
                        //        //if event is correct at this step
                        //        if (j.HasField("desc"))
                        //        {
                        //            string desc = j.GetField("desc").str;
                        //            Debug.Log(desc.ToUpper());
                        //            switch (desc.ToUpper())
                        //            {
                        //                case "KEEP STATE. WRONG EVENTDOSE":
                        //                    //
                        //                    mes.message = "Liều lượng không chính xác!!!";
                        //                    PhotonNetwork.RaiseEvent(PhotonEventCodes.EVENT_RESPONSE_MESSAGE, ConvertData.ObjectToByteArray(mes), raiseEventOptions, sendOptions);
                        //                    return;

                        //            }
                        //        }
                        //        return;
                        //    }
                        //}

                        PhotonNetwork.RaiseEvent(PhotonEventCodes.EVENT_RESPONSE_MESSAGE, ConvertData.ObjectToByteArray(mes), raiseEventOptions, sendOptions);
                        return;
                   
            } else
            {
                
            }
        }
    }

    //On GET STATE API finish 
    public void OnReceiveNextState(string text)
    {
        if (text == "")
        {
            Debug.Log("Lỗi kết nối");
        }

        else
        {
            JSONObject j = new JSONObject(text);
            if (j.GetField("code").i != 200)
            {
                Debug.Log("Lỗi kết nối: " + j.GetField("desc").str);
            }
            else
            {
                CancelInvoke();

                State state = new State();
                JSONObject parameters = j.GetField("parameter").list[0];
                state.id = parameters.GetField("state_id").i;
                state.name = parameters.GetField("state_name").str;
                state.code = parameters.GetField("state_code").str;

                List<JSONObject> lists = parameters.GetField("state_params").list;

                for (int i = 0; i < lists.Count; i++)
                {
                    state.addStateDetails(lists[i]);
                }

                if(state.code.Contains("P_"))
                {
                    //handle sub state P
                    CoroutineForStateP(parameters.GetField("state_timeout").f);
                } else
                {

                    //Display STATE after 1 second
                    StartCoroutine(HandleNextState(state, 1));
                }
                
            }

        }
    }

    public void CoroutineForStateP(float timeout)
    {
        //handle sub state P
        StartCoroutine(HandleStateP(10, timeout));
    }

    IEnumerator HandleStateP(float delaytime, float timeout)
    {
        yield return new WaitForSeconds(delaytime);

        while(GameStateController.instance.roomstate == 2)
        {
            yield return new WaitForSeconds(1);
        }

        //send next state P event
        NhiVR.Event action = new NhiVR.Event(33, "Chuyển trạng thái phụ", 20, "Chuyển trạng thái phụ" ,"E0");
        action.parameters = new Parameter(1, "", "");
        action.parameters.value = 1;
        StartCoroutine(APISendEvent(action, action.delaySend));

        //skip time
     

        //timeout to datetime format
        TimeSpan ts = TimeSpan.FromSeconds(timeout);
        string timeText = "";

        if(ts.Hours != 0)
        {
            timeText += ts.Hours + " giờ";
        }

        if(ts.Minutes != 0)
        {
            timeText += ts.Minutes + " phút";
        }

        if(ts.Seconds != 0)
        {
            timeText += ts.Seconds + " giây";
        }

        TextAnnounce.instance.ShowTextAnnounce(timeText + " sau ...");

        yield return null;
    }

    IEnumerator HandleNextState(State state, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        StateHistorySynchronize.instance.SynchronizeStateHistory(new StateHistory(DateTime.Now, state));

        yield return null;
    }

    public void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }


}
