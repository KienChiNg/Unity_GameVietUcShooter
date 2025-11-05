using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeartPressPopupController : APIEventPopupController
{
    // Update is called once per frame
    void Update()
    {
        //execute event when pressing "SPACE" key
        var pointer = new PointerEventData(EventSystem.current);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ExecuteEvents.Execute(executeButton.gameObject, pointer, ExecuteEvents.pointerDownHandler);
            
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            ExecuteEvents.Execute(executeButton.gameObject, pointer, ExecuteEvents.pointerUpHandler);
            ExecuteEvents.Execute(executeButton.gameObject, pointer, ExecuteEvents.pointerClickHandler);
        }
    }

    public override void ClosePopup()
    {
        //close popup
        if (closeEffect.DisplayEffect(gameObject, false))
        {
            UIManager.instance.actionPanelInuse = false;
            //hide tool
            if (GameObject.Find(action.eventcode))
            {
                GameObject.Find(action.eventcode).GetComponent<HeartPressModelController>().HideTool();
            }
        }
    }
}
