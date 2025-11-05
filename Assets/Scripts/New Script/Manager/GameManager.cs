

using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static GameManager instance;
    public GameObject heartCompressionPosition;
    public GameObject ambuMaskPosition;

    [Header("UI Role")]
    public Image roleTextImage;
    public Sprite chestCompressionSprite;
    public Sprite ambuMaskSprite;

    string heartCompressionPlayerId = "";
    string ambuMaskPositionPlayerId = "";
    bool isLostConnection = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (!PhotonNetwork.IsConnected) return;
        if (!PhotonNetwork.IsMasterClient) // 2
        {
            Invoke("InitObject", 0.2f);
        }
    }
    void InitObject()
    {
        if (roleTextImage == null) return;

        GameRole role = (GameRole)PhotonNetwork.LocalPlayer.CustomProperties["gameRole"];

        switch (role)
        {
            case GameRole.CHEST_COMPRESSION:
                roleTextImage.sprite = chestCompressionSprite;
                break;
            case GameRole.AMBU_MASK:
                roleTextImage.sprite = ambuMaskSprite;
                break;
        }
    }

    // Photon Methods
    //call when a player left room
    public override void OnPlayerLeftRoom(Player other)
    {
        //announce name of player leaving room
        TextAnnounce.instance.ShowTextAnnounce("Có người chơi đã rời khỏi phòng.");
        if (PhotonNetwork.IsMasterClient)
        {
            //MASTER CLIENT leave room when ROOM is closed
            if (PhotonNetwork.CurrentRoom.IsVisible == false && GameStateController.instance.roomstate == 99)
            {
                //LessonDetailManager.instance.EndLesson();
            }
        }

        //show message popup when MASTER CLIENT is disconnected
        if (((string)PhotonNetwork.CurrentRoom.CustomProperties["roomMaster"]).Equals(other.UserId)) // && !LessonDataSynchronize.instance.isHealthy())
        {
            LoadingEff.instance.ShowLoading("Chủ phòng đang mất kết nối. Vui lòng đợi trong giây lát...");
        }

    }

    public void QuitRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    //call when Master Client is Switched
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //if new Master Client is the one who created room
        //reset SimbabyState and Start listening to SERVER again
        if (PhotonNetwork.IsMasterClient && newMasterClient.UserId.Equals((string)PhotonNetwork.CurrentRoom.CustomProperties["roomMaster"]))
        {
            //get current room state from SERVER
            GameStateController.instance.GetRoomState();
            //start listening to WebSocket server

            EventDataSynchronize.instance.allowSend = true;
            //display current simbaby state
            //SimbabyController.instance.displayStateDetails(LessonDataSynchronize.instance.currentState);
        }
    }

    //call when user leaves room
    public override void OnLeftRoom()
    {
        if (isLostConnection == false)
        {
            //stop listening to WebSocket Server if is Master Client
            if (PhotonNetwork.IsMasterClient)
            {

            }

            //return to Launcher Scene
            PhotonNetwork.LoadLevel("Launcher");
        }

    }

    //call when a player Enters room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //announce name of player entering room
        TextAnnounce.instance.ShowTextAnnounce("Có người chơi mới đã tham gia phòng.");

        //Show popup in Master Client Scene to allow/disallow player enter room
        if (PhotonNetwork.IsMasterClient)
        {
            if (((string)PhotonNetwork.CurrentRoom.CustomProperties["roomMaster"]).Equals(newPlayer.UserId))
            {
                PhotonNetwork.SetMasterClient(newPlayer);
            }
            else
            {
                ExitGames.Client.Photon.Hashtable newPlayerProps = newPlayer.CustomProperties;
                if (heartCompressionPlayerId.Equals(""))
                {
                    newPlayerProps["gameRole"] = GameRole.CHEST_COMPRESSION;
                    heartCompressionPlayerId = newPlayer.UserId;
                }
                else if (ambuMaskPositionPlayerId.Equals(""))
                {
                    newPlayerProps["gameRole"] = GameRole.AMBU_MASK;
                    ambuMaskPositionPlayerId = newPlayer.UserId;
                }

                newPlayer.SetCustomProperties(newPlayerProps);

                if (!heartCompressionPlayerId.Equals("") && !ambuMaskPositionPlayerId.Equals(""))
                {
                    GameStateManager.instance.CanStartGame();
                }
            }
        }

        //hide message popup if room master is reconnected
        if (((string)PhotonNetwork.CurrentRoom.CustomProperties["roomMaster"]).Equals(newPlayer.UserId))
        {
            LoadingEff.instance.HideLoading();
        }

    }

    //API to add room member in SERVER
    public void AddRoomMember(Player player)
    {
        //JSONObject form = new JSONObject();
        //form.AddField("use_session", Common.instance.sessionId);
        //form.AddField("room_id", Room.instance.roomId);

        //JSONObject mem = new JSONObject();
        //mem.AddField("use_session", Common.instance.sessionId);
        //mem.AddField("use_name", player.CustomProperties["memUserName"].ToString());

        //JSONObject room_mem = new JSONObject();
        //room_mem.Add(mem);

        //form.AddField("room_member", room_mem);

        //StartCoroutine(APIController.instance.PostData(gameObject, "OnAddRoomMember", "/lesson/addroommember", form.ToString(), true, 30f));
    }

    //call when user is disconnected
    public override void OnDisconnected(DisconnectCause cause)
    {
        //reconnect to Room 
        isLostConnection = true;
        StartCoroutine(ReConnecting());
    }

    //call when rejoin room failed
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + " " + message);
        //if room is not exist 
        if (returnCode.Equals(32758))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //WebSocketController.instance.StopListening();
            }

            PhotonNetwork.LoadLevel("Launcher");
        }

    }

    //call when user joins room
    public override void OnJoinedRoom()
    {
        //hide message popup after user is reconnected
        if (isLostConnection)
        {
            isLostConnection = false;
            LoadingEff.instance.HideLoading();
        }
    }

    //reconnect function
    IEnumerator ReConnecting()
    {
        //show message popup
        LoadingEff.instance.ShowLoading("Mất kết nối. Đang kết nối lại...");
        //reconnect
        PhotonNetwork.ReconnectAndRejoin();

        //wait until user is connected
        while (!PhotonNetwork.IsConnected)
        {
            yield return new WaitForSeconds(10);
            Debug.Log("Connecting");
        }

        Debug.Log("Connected");

        //isLostConnection = false;

        yield return null;
    }
}
