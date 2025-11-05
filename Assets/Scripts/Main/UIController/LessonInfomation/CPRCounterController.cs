using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPRCounterController : MonoBehaviour
{
    public static CPRCounterController instance;
    float timer; // thời gian đếm ngược một chu kỳ CPR
    public Text timerText;
    public Text pressCountText;
    public Text pressSpeedText;
    public Text bopBongCountText;
    int pressCount;
    int bopbongCount;
    
    float timer0; //thời gian đếm ngược nhịp tim về 0 khi buông tay
    float deltaTime0 = 2; 

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        timer = 0;
        pressCount = 0;
        bopbongCount = 0;
        timer0 = 0;
        gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            //khi kết thúc đếm ngược tất cả tham số về 0
            timerText.text = "00:00:00";
            pressCountText.text = "0";
            bopBongCountText.text = "0";
            bopbongCount = 0;
            pressCount = 0;
        } else
        {
            timer -= Time.deltaTime;
            timerText.text = TimeSpan.FromSeconds(timer).ToString("mm\\:ss\\:ff");
            pressCountText.text = pressCount.ToString();
            bopBongCountText.text = bopbongCount.ToString();

            //Đếm ngược thời gian buông tay
            if (timer0 >= 0)
            {
                timer0 -= Time.deltaTime;
            }
        }

    }

    public void UpdateHeartPressSpeed()
    {
        if (timer <= 0)
        {
            //khi kết thúc đếm ngược tốc độ ép tim về 0
            pressSpeedText.text = "0";
        }
        else
        {
            //Sau 2s nếu không ép tim tốc độ ép tim về 0
            if (timer0 < 0)
            {
                //Cập nhật tốc độ ép tim về 0
                pressSpeedText.text = "0";

            }
            else
            {
                //Tính tốc độ ép tim/nhịp tim
                int hr = Mathf.RoundToInt(pressCount * 60 / (120 - timer));

                //Cập nhật tốc độ ép tim
                pressSpeedText.text = hr.ToString();

            }
        }
    }

    public void BopBongCounter()
    {
        bopbongCount += 1;
    }

    public void PressCounter()
    {
        if(pressCount == 0)
        {
            //Set thời gian đếm ngược 2p và số lần bóp bóng bằng 0
            timer = 120;
            bopbongCount = 0;
        }

        pressCount += 1;
        timer0 = deltaTime0;
    }

    public void OpenPopup()
    {
        //init parameters
        gameObject.SetActive(true);
        timerText.text = "00:00:00";
        pressCountText.text = "0";
        bopBongCountText.text = "0";
        pressSpeedText.text = "0";
        bopbongCount = 0;
        pressCount = 0;
        timer = 0;
    }

    public void ClosePopup()
    {
        gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        InvokeRepeating("UpdateHeartPressSpeed", 0, 2f);
    }

    private void OnDisable()
    {
        CancelInvoke();

    }
}
