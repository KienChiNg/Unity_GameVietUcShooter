using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState { WAITING, READY, START };
public class GameStateManager : MonoBehaviour, IOnEventCallback
{
    public static GameStateManager instance;
    bool canStart = false;
    GameStatManager statManager;
    [Header("UI Start Phase")]
    public Image announceImage;
    public Sprite onWaiting;
    public Sprite onReady;
    public Sprite onStart;
    public Button startBtn;
    public GameObject StartPhaseUI;

    private void Awake()
    {
        instance = this;
        statManager = GetComponent<GameStatManager>();
        if (announceImage != null)
        {
            announceImage.sprite = onWaiting;
        }

    }

    public void CanStartGame()
    {
        canStart = true;
        if (announceImage != null)
        {
            announceImage.sprite = onReady;
        }
        //
        if (startBtn != null)
        {
            startBtn.gameObject.SetActive(true);
            startBtn.onClick.RemoveAllListeners();
            startBtn.onClick.AddListener(delegate
            {
                announceImage.sprite = onStart;
                SendEvent(PhotonEventCodes.START_GAME);
                startBtn.gameObject.SetActive(false);
            });
        }

    }
    public void SetTextStartGame()
    {
        announceImage.sprite = onStart;
    }
    public void SetTextWattingGame()
    {
        announceImage.sprite = onWaiting;
    }
    // Send event to All
    public void SendEvent(byte code)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(code, "", raiseEventOptions, sendOptions);
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            //call when Other players send EVENT to MASTER CLIENT 
            case PhotonEventCodes.START_GAME:
                if (PhotonNetwork.IsMasterClient)
                {

                }
                // StartPhaseUI.SetActive(false);
                statManager.StartGamePhaseUI();
                break;

            //call when master client synchronize animation
            case PhotonEventCodes.PAUSE_GAME:


                break;

            //call when synchonize respone message
            case PhotonEventCodes.END_GAME:
                GameStatManager.instance.ShowEndgameResult();
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
        //get previous data when join room late
        if (!PhotonNetwork.IsMasterClient)
        {

        }
    }
}
