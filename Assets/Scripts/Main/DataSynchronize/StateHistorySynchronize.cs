using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateHistorySynchronize : MonoBehaviour, IOnEventCallback
{
    public static StateHistorySynchronize instance;
    public SortedList listState;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        listState = new SortedList();
    }

    //add state history
    public void addStateHistory(StateHistory stateHistory)
    {
        var key = stateHistory.time.Ticks;
        //prevent case key is duplicated 
        while (listState.ContainsKey(key))
        {
            key++;
        }
        //add state to sorted list
        listState.Add(key, stateHistory);

        //update UI follow current list state
        PatientInfoPanelController.instance.updateStatusBar(listState);
    }

    //synchronize new state history
    public void SynchronizeStateHistory(StateHistory stateHistory)
    {
        //add current state history to list
        addStateHistory(stateHistory);

        //MASTER CLIENT send new history state to all player
        if (PhotonNetwork.IsMasterClient)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(PhotonEventCodes.UPDATE_STATE_HISTORY, ConvertData.ObjectToByteArray(stateHistory), raiseEventOptions, sendOptions);
        }
    }

    //send request to get current history states list to MASTER CLIENT
    void RequestCurrentStateHistory()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEventCodes.REQUEST_CURRENT_STATE_HISTORY, PhotonNetwork.LocalPlayer.ActorNumber, raiseEventOptions, sendOptions);
    }


    void SendCurrentStateHistory(int target)
    {
        //if target actor number is 0, send current history states list to all other players
        //else send current history states list to target actor number
        if (target == 0)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(PhotonEventCodes.SEND_CURRENT_STATE_HISTORY, ConvertData.ObjectToByteArray(listState), raiseEventOptions, sendOptions);
        }
        else if (target > 0)
        {
            int[] tar = { target };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = tar };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(PhotonEventCodes.SEND_CURRENT_STATE_HISTORY, ConvertData.ObjectToByteArray(listState), raiseEventOptions, sendOptions);
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        switch(photonEvent.Code)
        {
            //call when new history state is SEND
            case PhotonEventCodes.UPDATE_STATE_HISTORY:
                byte[] data = (byte[])photonEvent.CustomData;
                //all player except MASTER CLIENT update new state history
                if (!PhotonNetwork.IsMasterClient)
                {
                    addStateHistory((StateHistory)ConvertData.ByteArrayToObject(data));
                }
                break;

            //call when current history states list is SEND
            case PhotonEventCodes.SEND_CURRENT_STATE_HISTORY:
                //all player except MASTER CLIENT update current history states list
                if (!PhotonNetwork.IsMasterClient)
                {
                    SortedList receiveListStateHistory = (SortedList)ConvertData.ByteArrayToObject((byte[])photonEvent.CustomData);
                    if (receiveListStateHistory.Count != 0)
                    {
                        listState = receiveListStateHistory;
                        PatientInfoPanelController.instance.updateStatusBar(listState);
                    }
                }

                break;

            //call when current state history list is REQUESTED
            case PhotonEventCodes.REQUEST_CURRENT_STATE_HISTORY:
                //MASTER CLIENT send current state history list to REQUESTING PLAYER
                if (PhotonNetwork.IsMasterClient)
                {
                    SendCurrentStateHistory((int)photonEvent.CustomData);
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

    //request current history state list after entering room
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            RequestCurrentStateHistory();
        }

    }
}
