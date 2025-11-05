using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Movement.AnimationRigging;
using Photon.Pun;
using UnityEngine;

public class MultiplayerCharacterController : MonoBehaviour
{
    public GameObject vrCamera;
    GameRole role = GameRole.NONE;
    private void Awake()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            vrCamera.SetActive(true);
            return;
        }

        ClearComponentInChildren(typeof(Camera));
        ClearComponentInChildren(typeof(AudioListener));
        ClearComponentInChildren(typeof(OVRCameraRig));
        ClearComponentInChildren(typeof(OVRManager));
        ClearComponentInChildren(typeof(OVRBody));
        ClearComponentInChildren(typeof(OVRHeadsetEmulator));
        ClearComponentInChildren(typeof(RetargetingLayer));
    }

    private void ClearComponentInChildren(Type type)
    {
        foreach (Component c in GetComponentsInChildren(type))
        {
            Destroy(c);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!GetComponent<PhotonView>().IsMine) return;

        GameRole _role = (GameRole)PhotonNetwork.LocalPlayer.CustomProperties["gameRole"];
        role = _role;
        switch (role)
        {
            case GameRole.CHEST_COMPRESSION:
            Debug.Log("Chest Compression Role");
                transform.localPosition = GameManager.instance.heartCompressionPosition.transform.localPosition;
                transform.localRotation = GameManager.instance.heartCompressionPosition.transform.localRotation;
                break;
            case GameRole.AMBU_MASK:
                transform.localPosition = GameManager.instance.ambuMaskPosition.transform.localPosition;
                transform.localRotation = GameManager.instance.ambuMaskPosition.transform.localRotation;
                break;
        }
    }
}
