using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class EventSynchronizeManager : xrInput, IOnEventCallback
{
    public static EventSynchronizeManager instance;
    public bool canTrigger = true;
    float delayTime = 0;
    bool isCheck = false;
    bool isWin = false;
    void Awake()
    {
        if (instance == null)
            instance = this;
    }
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        targetDeviceLeft.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue_L);
        targetDeviceLeft.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue_L);
        targetDeviceRight.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue_R);
        targetDeviceRight.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue_R);

        if (GameStatManager.instance.IsGamePlaying())
        {
            //     // if(delayTime > 0f) delayTime -= Time.deltaTime;
            delayTime += Time.deltaTime;
            // Debug.Log(delayTime);
            //     if (triggerValue_R > 0.1f)
            //     {
            //         // Debug.Log("aloo");
            //         if (!canTrigger)
            //         {
            //             return;
            //         }
            //         canTrigger = false;
            //         SendEvent(PhotonEventCodes.CHEST_COMPRESSION);


            //     }
            //     // else if (triggerValue_R < 0.1f && !primaryButtonValue_R)
            //     // {
            //     //     canTrigger = true;
            //     // }

        }
    }
    public void CheckShot(int ping)
    {
        if (GameStatManager.instance.IsGamePlaying())
        {
            // delayTime += Time.deltaTime;
            SendEvent(PhotonEventCodes.CHEST_COMPRESSION, ping);
        }
    }
    public void ReMatch()
    {

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEventCodes.REMATCH, "", raiseEventOptions, sendOptions);
        delayTime = 0;
        CancelInvoke("Waiting");
    }
    // Send event to All
    public void SendEvent(byte code, int ping)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            object[] content = new object[] { PhotonNetwork.LocalPlayer.UserId, ping };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(code, content, raiseEventOptions, sendOptions);
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            //call when Other players send EVENT to MASTER CLIENT 
            case PhotonEventCodes.CHEST_COMPRESSION:
                if (PhotonNetwork.IsMasterClient && !isCheck)
                {
                    isCheck = true;
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                    SendOptions sendOptions = new SendOptions { Reliability = true };
                    PhotonNetwork.RaiseEvent(PhotonEventCodes.WIN_PLAYER, photonEvent.CustomData, raiseEventOptions, sendOptions);
                    PhotonNetwork.RaiseEvent(PhotonEventCodes.WIN_PLAYER_TIME, delayTime, raiseEventOptions, sendOptions);
                }
                Debug.Log("photonEvent.CustomData: " + photonEvent.CustomData);
                break;

            //call when master client synchronize animation
            case PhotonEventCodes.AMBU_MASK:
                if (PhotonNetwork.IsMasterClient)
                {
                    GameStatManager.instance.IncreaseAmbuCount();
                }
                break;

            case PhotonEventCodes.WIN_PLAYER:
                int index = -1;
                object[] data = (object[])photonEvent.CustomData;
                if (PhotonNetwork.IsMasterClient)
                {
                    GameStatManager.instance.SetPanelGameUIWin();

                    // panelWin.SetTimeShot(delayTime);
                    GameStatManager.instance.SetPingForPlayerWinner((int)data[1]);
                }
                else
                {

                    if ((string)data[0] == PhotonNetwork.LocalPlayer.UserId)
                    {
                        Debug.Log("You win!");
                        GameRole _role = (GameRole)PhotonNetwork.LocalPlayer.CustomProperties["gameRole"];
                        // Debug.Log(_role);

                        switch (_role)
                        {
                            case GameRole.CHEST_COMPRESSION:
                                index = 1;
                                break;
                            case GameRole.AMBU_MASK:
                                index = 2;
                                break;
                        }

                        GameStatManager.instance.SetGameWin(true, PhotonNetwork.GetPing());
                        isWin = true;

                        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                        SendOptions sendOptions = new SendOptions { Reliability = true };
                        PhotonNetwork.RaiseEvent(PhotonEventCodes.WIN_PLAYER_INDEX, index, raiseEventOptions, sendOptions);
                    }
                    else
                    {
                        Debug.Log("You lose!");
                        GameStatManager.instance.SetGameWin(false, PhotonNetwork.GetPing());
                    }
                }

                break;
            case PhotonEventCodes.WIN_PLAYER_INDEX:
                if (PhotonNetwork.IsMasterClient)
                {
                    PanelWin panelWin = GameStatManager.instance.panelGameUIWin.GetComponent<PanelWin>();
                    panelWin.SetNamePlayerWin((int)photonEvent.CustomData);
                    // SetPingWinner();
                }
                break;
            case PhotonEventCodes.WIN_PLAYER_TIME:
                if (PhotonNetwork.IsMasterClient)
                {
                    PanelWin panelWin = GameStatManager.instance.panelGameUIWin.GetComponent<PanelWin>();
                    panelWin.SetTimeShot((float)photonEvent.CustomData);
                    panelWin.gameObject.SetActive(true);
                }
                else
                {
                    if (isWin)
                        GameStatManager.instance.SetGameWin(true, (float)photonEvent.CustomData);
                }
                break;
            case PhotonEventCodes.REMATCH:
                if (PhotonNetwork.IsMasterClient)
                {
                    GameStatManager.instance.ReMatchMaster();
                    isCheck = false;
                    delayTime = 0;
                    canTrigger = true;
                    isWin = false;
                }
                else
                {
                    GameStatManager.instance.ReMatchClient();
                    delayTime = 0;
                    canTrigger = true;
                    isWin = false;
                }
                break;
                // case PhotonEventCodes.PING_PLAYER:
                //     object[] data = (object[])photonEvent.CustomData;
                //     EventSynchronizeCharacter character = (EventSynchronizeCharacter)data[0];
                //     float ping = (float)data[1];
                //     character.DisplayStat(ping);
                //     break;
        }
    }
    // public void SetPingPlayer(EventSynchronizeCharacter character, float ping)
    // {
    //     object[] content = new object[] { character, ping };
    //     RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
    //     SendOptions sendOptions = new SendOptions { Reliability = true };
    //     PhotonNetwork.RaiseEvent(PhotonEventCodes.PING_PLAYER, content, raiseEventOptions, sendOptions);
    // }
    [PunRPC]
    public void SetPingWinner()
    {
        InvokeRepeating("Waiting", 0f, 1f);
    }
    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(1);
        GameStatManager.instance.SetPingForPlayerWinner(PhotonNetwork.GetPing());
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
