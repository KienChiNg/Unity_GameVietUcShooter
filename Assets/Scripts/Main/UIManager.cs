using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public bool actionPanelInuse = false;

    [Header("History")]
    public GameObject panelHistory;
    public GameObject btnHistory;

    [Header("Patient info")]
    public GameObject panelPatientInfo;
    [Header("Room member")]
    public GameObject panelRoomMember;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        panelPatientInfo.SetActive(false);
    }

    //open history action panel
    public void OpenHistoryPanel()
    {
        HistoryLogManager.instance.OpenPopup();
    }

    
    //close history action panel
    public void CloseHistoryPanel()
    {
        HistoryLogManager.instance.ClosePopup();
    }

    //open Patient Info and State history panel
    public void OpenPatientInfoPanel()
    {
        PatientInfoPanelController.instance.OpenPopup();
    }

    public void ActivePatientInfoPanel()
    {
        panelPatientInfo.SetActive(true);
        panelPatientInfo.transform.localScale = new Vector3(1, 1, 1);
        panelPatientInfo.transform.localPosition = new Vector3(0, 0, 0);
    }

    //close Patient Info and State history panel
    public void ClosePatientInfoPanel()
    {
        PatientInfoPanelController.instance.ClosePopup();
    }

    //open room member panel
    public void OpenRoomMemberPanel()
    {
        RoomMemberListController.instance.OpenPopup();

    }

    //close room member panel
    public void CloseRoomMemberPanel()
    {
        RoomMemberListController.instance.ClosePopup();
   
    }

}
