

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;

public enum PhotonRole { MASTER, PLAYER };
public enum GameRole { NONE, CHEST_COMPRESSION, AMBU_MASK };
public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher instance;
    string gameVersion = "1";
    public PhotonRole photonRole;
    int maxPlayersPerRoom = 3;
    public UnityAction<List<RoomInfo>> OnRoomListChanged;

    [Header("Administrator Settings")]
    public TMP_InputField roomNameInput;
    public Image status;

    public List<GameObject> waitingPanel;
    [Header("Player Settings")]
    public RoomListUIController roomListUI;
    bool isInstancePlayer = false;

    void Start()
    {
        //1
        PlayerPrefs.DeleteAll();

        if (PhotonNetwork.IsConnected) return;

        //connect to photon
        ConnectToPhoton();
    }

    void Awake()
    {
        //4 
        PhotonNetwork.AutomaticallySyncScene = true;
        instance = this;
    }

    void ConnectToPhoton()
    {
        //connect to photon in asia region
        LoadingEff.instance.ShowLoading("Connecting...");
        PhotonNetwork.GameVersion = gameVersion; //1
        PhotonNetwork.NetworkingClient.AppId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;

        if (Common.instance.isOnline)
        {
            PhotonNetwork.ConnectToRegion("asia");
        }
        else
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.Server = Common.instance.ipAddress;
            PhotonNetwork.ConnectUsingSettings();
            //PhotonNetwork.NetworkingClient.LoadBalancingPeer.SerializationProtocolType = SerializationProtocol.GpBinaryV16;
            //PhotonNetwork.ConnectToMaster(Common.instance.ipAddress, 5055, "");
            Debug.Log(Common.instance.ipAddress + ":" + PhotonNetwork.PhotonServerSettings.AppSettings.Port);
        }

        // //2
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnClickCloseRoom()
    {
        GenerateMessageBox.instance.ShowMessageBox("Thông báo", "Bạn có chắc chắn muốn xóa phòng?", "Có", "Không", () => { CloseRoom(); }, null);
    }

    public void CloseRoom()
    {
        //hidden room
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.CurrentRoom.EmptyRoomTtl = 0;

        //kick all players in room
        foreach (Player p in PhotonNetwork.CurrentRoom.Players.Values)
        {
            PhotonNetwork.CloseConnection(p);
        }

        //close room
        if (PhotonNetwork.IsMasterClient)
        {

        }

    }

    //call when client is connected to photon
    public override void OnConnected()
    {
        // 1
        base.OnConnected();
        // 2
        LoadingEff.instance.ShowLoading("Connected to Photon!");
        Debug.Log(PhotonNetwork.CloudRegion);
    }

    //call when client lost connection
    public override void OnDisconnected(DisconnectCause cause)
    {
        // 3
        LoadingEff.instance.ShowLoading("Disconnected. Please check your Internet connection.");
    }


    public override void OnConnectedToMaster()
    {
        LoadingEff.instance.ShowLoading("Connected to Master!");
        Debug.Log("OnConnectedToMaster");

        //init user parameters
        ExitGames.Client.Photon.Hashtable customProps = new ExitGames.Client.Photon.Hashtable();
        customProps.Add("photonRole", photonRole);
        customProps.Add("gameRole", GameRole.NONE);
        PhotonNetwork.SetPlayerCustomProperties(customProps);
        Debug.Log(PhotonNetwork.LocalPlayer.UserId);

        LoadingEff.instance.ShowLoading("Joining to lobby...");
        //After we connected to Master server, join the default Lobby
        TypedLobby typedLobby = new TypedLobby(" ", LobbyType.Default); //3
        PhotonNetwork.JoinLobby(typedLobby);

    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        if (photonRole == PhotonRole.MASTER)
        {
            //create room
            LoadingEff.instance.ShowLoading("Creating room...");
            CreateRoom();
        }
        else
        {
            //show join room panel
            roomListUI.gameObject.SetActive(true);
            LoadingEff.instance.HideLoading();
        }
    }

    private void CreateRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            //admin create a photon room to start lesson
            RoomOptions roomOptions = new RoomOptions() { PublishUserId = true, MaxPlayers = maxPlayersPerRoom, PlayerTtl = 300000, EmptyRoomTtl = 0, CleanupCacheOnLeave = false }; //2
            TypedLobby typedLobby = new TypedLobby(" ", LobbyType.Default); //3

            PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.UserId, roomOptions, typedLobby, null); //4
        }
    }

    //call when a room is created or deleted
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("We have received the Room list " + roomList.Count);
        OnRoomListChanged?.Invoke(roomList);
    }

    //call when user join room failed
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + " " + message);
    }

    //call when user join room successfully
    public override void OnJoinedRoom()
    {
        Debug.Log("join room");

        // 4
        if (PhotonNetwork.IsMasterClient)
        {
            //master client of room init room parameters
            ExitGames.Client.Photon.Hashtable customProps = new ExitGames.Client.Photon.Hashtable();
            customProps.Add("roomMaster", PhotonNetwork.LocalPlayer.UserId);
            customProps.Add("roomName", "roomname");
            PhotonNetwork.CurrentRoom.SetCustomProperties(customProps);
            PhotonNetwork.CurrentRoom.SetPropertiesListedInLobby(new string[] { "roomName" });
            //
            status.color = new Color32(108, 233, 52, 255);
            roomNameInput.interactable = true;
            roomNameInput.onEndEdit.AddListener(delegate
            {
                ExitGames.Client.Photon.Hashtable customProps = PhotonNetwork.CurrentRoom.CustomProperties;
                customProps["roomName"] = roomNameInput.text;
                PhotonNetwork.CurrentRoom.SetCustomProperties(customProps);

            });
            LoadingEff.instance.HideLoading();
        }
        else
        {
            if (!isInstancePlayer)
            {
                GameObject player = PhotonNetwork.Instantiate("Model/PlayerController", new Vector3(1.017f, 0, -7.574f), Quaternion.identity);
                player.name = PhotonNetwork.LocalPlayer.UserId;
                isInstancePlayer = true;
            }
        }

        this.enabled = false;
    }
    public void RemoveWaiting()
    {
        if (waitingPanel.Count > 0)
        {
            waitingPanel[0].SetActive(false);
            waitingPanel.RemoveAt(0);
        }
    }
    //call when a player join room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

    }

    //call when a player leave room
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {

        if (PhotonNetwork.IsMasterClient)
        {
            //master client leave room when all player is leave and roomstate is closed
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1 && PhotonNetwork.CurrentRoom.IsVisible == false)
            {
                PhotonNetwork.LeaveRoom();
            }
        }
    }

    //call when user leave room
    public override void OnLeftRoom()
    {

    }
}
