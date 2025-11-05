using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class GameStateController : MonoBehaviour, IPunObservable
{
    public static GameStateController instance;
    public GameObject block;
    public GameObject startBtn;
    public GameObject pauseBtn;
    public GameObject endBtn;
    public GameObject playBtn;
    public GameObject skipBtn;
    public PlayableDirector intro;
    public Button quitRoomBtn;

    public bool isPlayIntro = true;
    public int roomstate = -1;
    public GameObject baby;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //no action can be execute if block is active
        block.SetActive(true);

        if(PhotonNetwork.IsMasterClient)
        {
            //enable pause and start button and disable others IF is Master Client
            pauseBtn.SetActive(true);
            startBtn.SetActive(true);
            endBtn.SetActive(false);
            playBtn.SetActive(false);
            skipBtn.SetActive(false);
            quitRoomBtn.interactable = false;
        } else
        {
            //disable all button IF is not Master Client
            pauseBtn.SetActive(false);
            startBtn.SetActive(false);
            endBtn.SetActive(false);
            playBtn.SetActive(false);
            skipBtn.SetActive(false);
        }
    }

    //Get current room state from Server
    public void GetRoomState()
    {
        JSONObject form = new JSONObject();

        form.AddField("room_id", Room.instance.roomId);

        StartCoroutine(APIController.instance.PostData(gameObject, "OnGetRoomState", "/lesson/getroomstate", form.ToString(), true, 30f));
    }

    //run Patient info audio
    public void ReadPatientInfo()
    {
        //set room state to 3 and synchronize
        if(PhotonNetwork.IsMasterClient)
        {
            roomstate = 3;
        }
        //
        block.SetActive(false);
        //disable start btn and end btn 
        startBtn.SetActive(false);
        endBtn.SetActive(false);

        //play patient info audio
        if (GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().Play();
        }

        //open patient info panel
        UIManager.instance.ActivePatientInfoPanel();
        //run StartPrepare() function after 10 seconds
        Invoke("StartPrepare", 10f);
    }

    public void StartPrepare()
    {
        //enable skip button for master client
        if(PhotonNetwork.IsMasterClient)
        {
            skipBtn.SetActive(true);
        }
        //display text announce
        TextAnnounce.instance.ShowTextAnnounce("Bắt đầu 2 phút chuẩn bị cấp cứu...");
        
        //start count down 120 seconds
        TimerController.instance.StartCountDown(120);
        
        //run StartGame() function after 120 seconds
        Invoke("StartGame", 120f);
    }

    public void SkipPrepare()
    {
        //set room state to 4 and synchronize
        if (PhotonNetwork.IsMasterClient)
        {
            roomstate = 4;
        }
        //Cancel all exist Invoke
        CancelInvoke();
        StartGame();
    }

    public void StartGame()
    {
        //if local player is Master client, Send API to start room
        if(PhotonNetwork.IsMasterClient)
        {
            JSONObject form = new JSONObject();

            form.AddField("room_id", Room.instance.roomId);
            StartCoroutine(APIController.instance.PostData(gameObject, "OnGameStateChange", "/lesson/startroom", form.ToString(), false, 30f));
        }
        //play intro
        if(intro)
        {
            intro.gameObject.SetActive(true);
            intro.Play();
        }
      
    }

    public void OnGameStateChange(string text)
    {
        //API get current room state
        if(text != "")
        {

        }
    }

    public void ContinueGame()
    {
        //API continue game

    }


    public void PauseGame()
    {

    }

    public void StopGame()
    {
        //API stop game

        
    }

    public void OnGetRoomState(string text)
    {
        if(text != "")
        {
            //update room state
            JSONObject j = new JSONObject(text);
            roomstate = (int)j.GetField("room_state").i; 

            switch(roomstate)
            {
                //case room is not started
                case 0:
                    isPlayIntro = false;
                    block.SetActive(true);
                    pauseBtn.SetActive(true);
                    startBtn.SetActive(true);
                    endBtn.SetActive(false);
                    playBtn.SetActive(false);
                    quitRoomBtn.interactable = false;
                    break;

                //case room is running
                case 1:
                    ContinueGameAction();
                    startBtn.SetActive(false);
                    endBtn.SetActive(true);
                    pauseBtn.SetActive(true);
                    playBtn.SetActive(false);
                    pauseBtn.GetComponent<Button>().interactable = true;
                    break;

                //case room is paused
                case 2:
                    PauseGameAction();
                    pauseBtn.SetActive(false);
                    playBtn.SetActive(true);
                    break;
    
                //case room is stopped
                case 99:
                    PhotonNetwork.CurrentRoom.IsVisible = false;
                    PhotonNetwork.CurrentRoom.EmptyRoomTtl = 0;
                    quitRoomBtn.interactable = true;
                    if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
                    {
         
                    }
                    break;
            }
        }
    }

    public void PauseGameAction()
    {
        //pause count down and block all user click
        block.SetActive(true);
        TimerController.instance.PauseCountDown();
    }

    public void ContinueGameAction()
    {
        //continue count down and allow user click
        baby.transform.localScale = new Vector3(1, 1, 1);
        TimerController.instance.ContinueCountDown();
        block.SetActive(false);
        CancelInvoke();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            //master client sends room state
            stream.SendNext(roomstate);
        } else
        {
            //other players receive room state
            if(!PhotonNetwork.IsMasterClient)
            {
                //update room state
                roomstate = (int)stream.ReceiveNext();
                switch(roomstate)
                {
                    //case game is not started
                    case 0:
                        isPlayIntro = false;
                        block.SetActive(true);
                        startBtn.SetActive(false);
                        endBtn.SetActive(false);
                        pauseBtn.SetActive(false);
                        playBtn.SetActive(false);
                        break;

                    //case game is continued
                    case 1:
                        ContinueGameAction();
                        break;

                    //case game is paused
                    case 2:
                        PauseGameAction();
                        break;

                    //case start read patient info
                    case 3:
                        if(!isPlayIntro)
                        {
                            ReadPatientInfo();
                            isPlayIntro = true;
                        }
                        break;

                    //case skip prepare
                    case 4:
                        isPlayIntro = false;
                        if (!isPlayIntro)
                        {
                            SkipPrepare();
                            isPlayIntro = true;
                        }
                        break;

                    //case room is stopped
                    case 99:
                        PhotonNetwork.LeaveRoom();
                        break;
                }
            }
        }
    }
}
