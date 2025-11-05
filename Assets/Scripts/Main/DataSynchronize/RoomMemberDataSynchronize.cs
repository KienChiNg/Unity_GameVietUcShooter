using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomMemberDataSynchronize : MonoBehaviour, IOnEventCallback
{
    public static RoomMemberDataSynchronize instance;
    List<User> users;

    private void Awake()
    {
        instance = this;
    }

    //update users list after getting from API
    public void UpdateListUsers(List<JSONObject> jo)
    {
        users = new List<User>();
        for(int i = 0; i < jo.Count; i ++)
        {
            User user = new User();
            if (jo[i].HasField("use_name"))
            {
                user.userName = jo[i].GetField("use_name").str;
            }
            
            if(jo[i].HasField("use_role"))
            {
                user.memRole = (int)jo[i].GetField("use_role").i;
            }
            
            users.Add(user);
        }

        //update to UI
        RoomMemberListController.instance.UpdateRoomMemberList(users);

        //send to other players
        SendCurrentListUser(0);
    }

    //send request to get current users list to Master Client
    void RequestCurrentListUser()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEventCodes.REQUEST_CURRENT_LIST_USER, PhotonNetwork.LocalPlayer.ActorNumber, raiseEventOptions, sendOptions);
    }

    //send current users list to other players
    void SendCurrentListUser(int target)
    {
        //if target actor number is 0, send users list to all player
        //else send users list to target actor number
        if (target == 0)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(PhotonEventCodes.SEND_CURRENT_LIST_USER, ConvertData.ObjectToByteArray(users), raiseEventOptions, sendOptions);
        }
        else if (target > 0)
        {
            int[] tar = { target };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = tar };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(PhotonEventCodes.SEND_CURRENT_LIST_USER, ConvertData.ObjectToByteArray(users), raiseEventOptions, sendOptions);
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            //call when current users list is SEND
            case PhotonEventCodes.SEND_CURRENT_LIST_USER:

                //all player except MASTER CLIENT update current users list
                if (!PhotonNetwork.IsMasterClient)
                {
                    users = (List<User>)ConvertData.ByteArrayToObject((byte[])photonEvent.CustomData);
                    RoomMemberListController.instance.UpdateRoomMemberList(users);
                }

                break;

            //call when current users list is REQUESTED
            case PhotonEventCodes.REQUEST_CURRENT_LIST_USER:

                //MASTER CLIENT send current users list
                if (PhotonNetwork.IsMasterClient)
                {
                    SendCurrentListUser((int)photonEvent.CustomData);
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
        //get current users list after entering room  
        if (!PhotonNetwork.IsMasterClient)
        {
            RequestCurrentListUser();
        }

    }
}
