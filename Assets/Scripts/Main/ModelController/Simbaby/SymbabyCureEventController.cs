using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SymbabyCureEventController : MonoBehaviour
{
    public static SymbabyCureEventController instance;
    public GameObject popup_eptim;

    private void Awake()
    {
        instance = this;
    }

    private void OnMouseDown()
    {
        if(!UIEvent.instance.IsPointerOverUIObject() && !UIManager.instance.actionPanelInuse)
        {

        }
                  
    }

    //simbaby cure event popup
    public bool OpenPopup(NhiVR.Event action)
    {
        switch (action.eventcode)
        {
            //open popup to press heart
            case "E5":
                return popup_eptim.GetComponent<APIEventPopupController>().OpenPopup(action);

            default:
                ConfirmAction.instance.initConfirmDrug(action);

                return true;
        }
    }
}
