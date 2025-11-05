using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class HistoryLogSynchronize : MonoBehaviour, IOnEventCallback
{
    public static HistoryLogSynchronize instance;
    SortedList listLog;
    bool isSending;
    
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        listLog = new SortedList();
        isSending = false;
    }

    public SortedList getListLog()
    {
        return listLog;
    }

    public void SynchronizeHistoryLog(HistoryLog log, bool sync)
    {
        var key = log.time.Ticks;
        //prevent logs with the same key
        while (listLog.ContainsKey(key))
        {
            key++;
        }

        //add log to sorted list
        listLog.Add(key, log);

        //init history log prefab
        HistoryLogManager.instance.AddHistoryLog(log, listLog.Count - 1);

        //master client synchronize to others player  
        if (sync)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(PhotonEventCodes.HISTORY_LOG, ConvertData.ObjectToByteArray(log), raiseEventOptions, sendOptions);
            isSending = true;
        }
    }

    //send request to get ALL history log to MASTER CLIENT
    void RequestAllHistoryLog()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEventCodes.REQUEST_All_HISTORY_LOG, PhotonNetwork.LocalPlayer.ActorNumber, raiseEventOptions, sendOptions);
    }

    void SendAllHistoryLog(int target)
    {
        //if target actor number is 0, send ALL history log to ALL PLAYER
        //else send ALL history log to target actor number
        if (target == 0)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(PhotonEventCodes.SEND_All_HISTORY_LOG, ConvertData.ObjectToByteArray(listLog), raiseEventOptions, sendOptions);
        }
        else if (target > 0)
        {
            int[] tar = { target };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = tar };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(PhotonEventCodes.SEND_All_HISTORY_LOG, ConvertData.ObjectToByteArray(listLog), raiseEventOptions, sendOptions);
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            //call when a new history log is SEND
            case PhotonEventCodes.HISTORY_LOG:

                byte[] data = (byte[])photonEvent.CustomData;
                if (!isSending)
                {
                    SynchronizeHistoryLog((HistoryLog)ConvertData.ByteArrayToObject(data), false);
                }
                else
                {
                    isSending = false;
                }

                break;

            //call when all history log is SEND
            case PhotonEventCodes.SEND_All_HISTORY_LOG:

                if (!PhotonNetwork.IsMasterClient)
                {
                    SortedList receiveListLog = (SortedList)ConvertData.ByteArrayToObject((byte[])photonEvent.CustomData);
                    if (receiveListLog.Count != 0)
                    {
                        //save history logs
                        listLog = receiveListLog;
                        //display all history logs
                        HistoryLogManager.instance.ResetHistoryLog();
                    }
                }

                break;

            //call when all history log is REQUESTED
            case PhotonEventCodes.REQUEST_All_HISTORY_LOG:
                if (PhotonNetwork.IsMasterClient)
                {
                    SendAllHistoryLog((int)photonEvent.CustomData);
                }
                break;
        }
    }

    public void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    public void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        //get previous history log when join room late
        if (!PhotonNetwork.IsMasterClient)
        {
            RequestAllHistoryLog();
        }
        
    }

}
