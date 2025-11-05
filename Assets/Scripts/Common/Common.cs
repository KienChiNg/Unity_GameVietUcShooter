using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Common : MonoBehaviour
{
    //save all client and game info 
    public static Common instance;
    public bool isOnline = true;
    public string ipAddress;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
       

    }
    void Start()
    {
        instance = this;
        #if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
        #else
            Debug.unityLogger.logEnabled = false;
        #endif
        //PhotonNetwork.SendRate = 10;
        //PhotonNetwork.SerializationRate = 1;
    }

}


