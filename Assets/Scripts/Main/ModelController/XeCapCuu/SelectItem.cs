using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectItem : MonoBehaviour
{
    public static SelectItem instance ;
    public HoldingItem item;

    private void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        gameObject.SetActive(false);
    }
    //close popup and enable tools
    public void SelectButton()
    {
        if(item.start == false)
        {
            item.SetUpPosAndStart();

            gameObject.SetActive(false);
        }
    }
    //close popup function
    public void CloseUI()
    {
        gameObject.SetActive(false);
    }

    
}
