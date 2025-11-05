using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListUIController : MonoBehaviour
{
    public Transform roomContainer;
    public GameObject roomPrefab;
    Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();
    // Start is called before the first frame update
    private void Awake()
    {
        Launcher.instance.OnRoomListChanged += UpdateRoomList;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateRoomList(List<RoomInfo> roomInfos)
    {
        Debug.Log(roomInfos.Count);
        for (int i = 0; i < roomInfos.Count; i++) {

            RoomInfo info = roomInfos[i];
            if (info.RemovedFromList)
            {
                ClearRoom(roomDict[info.Name]);
                roomDict.Remove(info.Name);
                continue;
            }

            Transform room = null;

            if (roomDict.ContainsKey(info.Name))
            {
                room = roomDict[info.Name].transform;
            } else
            {
                for (int j = 0; j < roomContainer.childCount; j++) 
                {
                    Transform roomInContainer = roomContainer.GetChild(j);
                    if (roomInContainer.gameObject.activeSelf == false)
                    {
                        room = roomInContainer;
                    }
                }

                if (room == null)
                {
                    room = Instantiate(roomPrefab, roomContainer).transform;
                }
                roomDict.Add(info.Name, room.gameObject);
            }


            ExitGames.Client.Photon.Hashtable props = info.CustomProperties;
            room.GetChild(1).GetComponent<TMP_Text>().text = $"#{props["roomName"]}";
            TMP_Text playerCountText = room.GetChild(2).GetComponent<TMP_Text>();
            playerCountText.text = $"{info.PlayerCount - 1}/2";
            if(info.PlayerCount < 3 )
            {
                playerCountText.color = Color.green;
            } else
            {
                playerCountText.color = Color.red;
            }

            //
            room.GetComponent<Button>().onClick.AddListener(delegate
            {
                PhotonNetwork.JoinRoom(info.Name);
            });
            room.gameObject.SetActive(true);
        }
    }

    void ClearRoom(GameObject room)
    {
        room.SetActive(false);
        room.GetComponent<Button>().onClick.RemoveAllListeners();
    }
}
