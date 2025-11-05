using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class TimerController : MonoBehaviour, IPunObservable
{
    public static TimerController instance;
    float timer = 0;
    GameObject timerText;
    bool isSynchronizeTimer;
    bool canCount = false;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        timerText = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //count down timer to 0 when canCount is true 
        //and timer >= 0
        if (canCount == true && timer >= 0)
        {
            timer -= Time.deltaTime;
            timerText.GetComponent<Text>().text = TimeSpan.FromSeconds(timer).ToString("hh\\:mm\\:ss");
        }
        else if (timer < 0)
        {
            canCount = false;
            timerText.GetComponent<Text>().text = "00:00:00";
        }
    }

    //start count down a specific time
    public void StartCountDown(float time)
    {
        canCount = true;
        timer = time;
    }

    //pause count down
    public void PauseCountDown()
    {
        canCount = false;
    }

    //continue count down
    public void ContinueCountDown()
    {
        canCount = true;
    }

    //update timer
    public void setTimer(float timer)
    {
        this.timer = timer;
        timerText.GetComponent<Text>().text = TimeSpan.FromSeconds(timer).ToString("hh\\:mm\\:ss");
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    //client start synchronize time when joining room
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        isSynchronizeTimer = true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (PhotonNetwork.IsMasterClient && isSynchronizeTimer)
            {
                //send current timer
                stream.SendNext(timer);
            }
        } else
        {
            //update current timer if not master client
            if(!PhotonNetwork.IsMasterClient)
            {
                timer = (float)stream.ReceiveNext();
                timerText.GetComponent<Text>().text = TimeSpan.FromSeconds(timer).ToString("hh\\:mm\\:ss");
            }
        }
    }
}
