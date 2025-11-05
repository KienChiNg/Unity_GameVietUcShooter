using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public enum GamePhase { NOT_START, CHEST_COMPRESSION, AMBU_MASK, END_GAME }

public class GameStatManager : MonoBehaviour, IPunObservable
{
    public static GameStatManager instance;
    [Header("UI Game Phase")]
    public GameObject GamePhaseUI;
    public TMP_Text timeText;
    public TMP_Text phaseNameText;
    public TMP_Text playerNameText;
    public TMP_Text countText;

    public TMP_Text commandText;

    [Header("UI End Game Phase")]
    public EndGamePopupController endGamePopupController;
    //
    float defaultPhaseTime = 45;
    float time = 6;
    GamePhase currentPhaseType = GamePhase.NOT_START;
    static int phaseCount = 3;
    int currentPhase = 0;
    bool isShoot = false;

    GameResult result = new GameResult(phaseCount);

    int targetCompressionCount = 15;
    int targetAmbuCount = 2;

    public GameObject winGameUI;
    public GameObject loseGameUI;
    public GameObject panelGameUIWin;
    public GameObject reMatch;
    public List<string> listUserId = new List<string>();
    public GameObject startPhaseUI;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        isShoot = false;
    }
    public bool IsGamePlaying()
    {
        return isShoot;
    }
    public void ReMatchMaster()
    {
        isShoot = false;
        time = 6;
        isStarted = false;
        GameStateManager.instance.CanStartGame();
        panelGameUIWin.SetActive(false);
        // GamePhaseUI.SetActive(true);
    }
    public void ReMatchClient()
    {
        startPhaseUI.SetActive(true);
        isShoot = false;
        time = 6;
        isStarted = false;
        loseGameUI.SetActive(false);
        winGameUI.SetActive(false);
        GameStateManager.instance.SetTextWattingGame();
    }
    public void SetGameWin(bool isWin, float time = 0)
    {
        startPhaseUI?.SetActive(false);
        GamePhaseUI?.SetActive(false);
        if (isWin == true)
        {
            winGameUI.SetActive(true);
            winGameUI.GetComponent<PanelWin>().SetTimeShot(time);
            // winGameUI.GetComponent<PanelWin>().SetPing(ping);
            loseGameUI.SetActive(false);

        }
        else
        {
            loseGameUI.SetActive(true);
            winGameUI.SetActive(false);
        }
    }
    public void SetPingForPlayerWinner(float ping)
    {
        panelGameUIWin.GetComponent<PanelWin>().SetPing(ping); 
    }
    public void SetPanelGameUIWin()
    {
        panelGameUIWin.SetActive(true);
        GamePhaseUI.SetActive(false);
        reMatch.SetActive(true);
    }
    public void IncreaseCompressionCount()
    {
        Debug.Log("Compression");
        if (currentPhaseType == GamePhase.CHEST_COMPRESSION)
        {
            result.compressionCount[currentPhase]++;
            if (result.compressionCount[currentPhase] == targetCompressionCount)
            {
                Invoke(nameof(SwitchPhase), 1f);
            }
        }
        else if (currentPhaseType == GamePhase.AMBU_MASK)
        {
            result.wrongCompressionCount[currentPhase]++;
        }
    }

    public void IncreaseAmbuCount()
    {
        Debug.Log("ambu");
        if (currentPhaseType == GamePhase.AMBU_MASK)
        {
            result.ambuCount[currentPhase]++;
            if (result.ambuCount[currentPhase] == targetAmbuCount)
            {
                Invoke(nameof(SwitchPhase), 1f);
            }
        }
        else if (currentPhaseType == GamePhase.CHEST_COMPRESSION)
        {
            result.wrongAmbuCount[currentPhase]++;
        }
    }
    public void AddStackUser(string userId)
    {

    }
    bool isStarted = false;
    public void StartGamePhaseUI()
    {
        GameStateManager.instance.SetTextStartGame();
        GamePhaseUI.SetActive(true);
        currentPhaseType = GamePhase.CHEST_COMPRESSION;
        currentPhase = 0;
        time = 6;
        isStarted = true;
        // ResetPhaseStat();
    }

    public void SwitchPhase()
    {
        switch (currentPhaseType)
        {
            case GamePhase.CHEST_COMPRESSION:
                currentPhaseType = GamePhase.AMBU_MASK;
                ResetPhaseStat(false);
                break;

            case GamePhase.AMBU_MASK:
                if (currentPhase < phaseCount - 1)
                {
                    currentPhaseType = GamePhase.CHEST_COMPRESSION;
                    currentPhase++;
                    ResetPhaseStat();

                }
                else
                {
                    GameStateManager.instance.SendEvent(PhotonEventCodes.END_GAME);
                }
                break;
        }
    }

    public void ShowEndgameResult()
    {
        GamePhaseUI.SetActive(false);
        currentPhaseType = GamePhase.END_GAME;
        ResetPhaseStat(false);
        endGamePopupController.ShowResultPanel(result, targetCompressionCount, targetAmbuCount, phaseCount);
    }

    private void ResetPhaseStat(bool isResetCount = true)
    {

        if (isResetCount)
        {
            result.compressionCount[currentPhase] = 0;
            result.ambuCount[currentPhase] = 0;
        }

        switch (currentPhaseType)
        {
            case GamePhase.CHEST_COMPRESSION:
                targetCompressionCount = 15;
                targetAmbuCount = 0;
                time = 6;
                break;
            case GamePhase.AMBU_MASK:
                targetCompressionCount = 0;
                targetAmbuCount = 2;
                time = 6;
                break;
            case GamePhase.END_GAME:
                targetCompressionCount = 15;
                targetAmbuCount = 2;
                break;
        }
    }

    public void UpdateAdminUI()
    {
        timeText.text = $"{(int)time}s";
        if (time <= 0)
        {
            timeText.text = "Shoot!";
        }

        // switch (currentPhaseType)
        // {
        //     case GamePhase.CHEST_COMPRESSION:
        //         phaseNameText.text = "CHEST COMRPESSIONS";
        //         playerNameText.text = "Player1";
        //         phaseNameText.color = new Color32(39, 109, 176, 255);
        //         playerNameText.color = new Color32(39, 109, 176, 255);
        //         countText.text = $"{result.compressionCount[currentPhase]}/{targetCompressionCount}";
        //         break;
        //     case GamePhase.AMBU_MASK:
        //         phaseNameText.text = "AMBU BAG VENTILATION";
        //         playerNameText.text = "Player2";
        //         phaseNameText.color = new Color32(6, 134, 96, 255);
        //         playerNameText.color = new Color32(6, 134, 96, 255);
        //         countText.text = $"{result.ambuCount[currentPhase]}/{targetAmbuCount}";
        //         break;
        // }
    }

    public void UpdatePlayerUI(GameRole currentRole)
    {
        timeText.text = $"{(int)time}s";

        if (time <= 0)
        {
            timeText.text = "Shoot!";
            isShoot = true;
        }
        //update text
        // if(currentRole == GameRole.CHEST_COMPRESSION)
        // {
        //     //
        //     phaseNameText.text = "CHEST COMRPESSIONS";
        //     countText.text = $"{result.compressionCount[currentPhase]}/{targetCompressionCount}";
        //     if (result.compressionCount[currentPhase] > targetCompressionCount)
        //     {
        //         countText.color = new Color32(255, 43, 43, 255);
        //     }
        //     else
        //     {
        //         countText.color = new Color32(0, 136, 255, 255);
        //     }
        // }

        // if (currentRole == GameRole.AMBU_MASK)
        // {
        //     phaseNameText.text = "AMBU BAG VENTILATION";
        // countText.text = $"{result.ambuCount[currentPhase]}/{targetAmbuCount}";
        // if (result.ambuCount[currentPhase] > targetAmbuCount)
        // {
        //     countText.color = new Color32(255, 43, 43, 255);
        // }
        // else
        // {
        //     countText.color = new Color32(0, 136, 255, 255);
        // }
        // }


        //udpate command
        // switch (currentPhaseType)
        // {
        //     case GamePhase.CHEST_COMPRESSION:
        //         if(currentRole == GameRole.AMBU_MASK)
        //         {
        //             commandText.text = "WAIT FOR YOUR TURN";
        //             commandText.color = new Color32(255, 43, 43, 255);
        //         } else if(currentRole == GameRole.CHEST_COMPRESSION)
        //         {
        //             commandText.text = "YOUR TURN";
        //             commandText.color = new Color32(29, 200, 77, 255);
        //         }

        //         break;
        //     case GamePhase.AMBU_MASK:
        //         if (currentRole == GameRole.CHEST_COMPRESSION)
        //         {
        //             commandText.text = "WAIT FOR YOUR TURN";
        //             commandText.color = new Color32(255, 43, 43, 255);
        //         }
        //         else if (currentRole == GameRole.AMBU_MASK)
        //         {
        //             commandText.text = "YOUR TURN";
        //             commandText.color = new Color32(29, 200, 77, 255);
        //         }
        //         break;
        // }
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsConnected) return;

        // if (PhotonNetwork.IsMasterClient)
        // {
        UpdateAdminUI();
        GameRole currentRole = (GameRole)PhotonNetwork.LocalPlayer.CustomProperties["gameRole"];
        UpdatePlayerUI(currentRole);
        if (time > 0 && isStarted)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                time = 0;
                //do something
                // SwitchPhase();
            }
        }
        // }
        // else
        // {
        //     GameRole currentRole = (GameRole)PhotonNetwork.LocalPlayer.CustomProperties["gameRole"];
        //     UpdatePlayerUI(currentRole);
        // }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(time);

            stream.SendNext(currentPhaseType);
            stream.SendNext(currentPhase);

            stream.SendNext(targetAmbuCount);
            stream.SendNext(targetCompressionCount);

            stream.SendNext(result.ambuCount[currentPhase]);
            stream.SendNext(result.compressionCount[currentPhase]);

            stream.SendNext(result.wrongAmbuCount[currentPhase]);
            stream.SendNext(result.wrongCompressionCount[currentPhase]);
        }
        else if (stream.IsReading)
        {
            time = (float)stream.ReceiveNext();

            currentPhaseType = (GamePhase)stream.ReceiveNext();
            currentPhase = (int)stream.ReceiveNext();

            targetAmbuCount = (int)stream.ReceiveNext();
            targetCompressionCount = (int)stream.ReceiveNext();

            result.ambuCount[currentPhase] = (int)stream.ReceiveNext();
            result.compressionCount[currentPhase] = (int)stream.ReceiveNext();

            result.wrongAmbuCount[currentPhase] = (int)stream.ReceiveNext();
            result.wrongCompressionCount[currentPhase] = (int)stream.ReceiveNext();
        }
    }
}

public class GameResult
{
    public int[] compressionCount;
    public int[] ambuCount;

    public int[] wrongCompressionCount;
    public int[] wrongAmbuCount;

    public GameResult(int phaseCount)
    {
        compressionCount = new int[phaseCount];
        ambuCount = new int[phaseCount];

        wrongCompressionCount = new int[phaseCount];
        wrongAmbuCount = new int[phaseCount];
    }
}