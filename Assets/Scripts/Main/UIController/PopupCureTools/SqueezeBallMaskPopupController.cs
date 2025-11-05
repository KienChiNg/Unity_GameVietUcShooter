using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SqueezeBallMaskPopupController : APIEventPopupController
{
    public GameObject tool;
    public Dropdown input;

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
            tool.GetComponent<CureToolModelController>().HideTool();
        }
        
    }

    public override void OnClickExecute()
    {
        //get electric input value
        string resultStr = input.captionText.text;

        //set value to action 
        float result;
        if (float.TryParse(resultStr, out result))
        {
            action.parameters.value = 500;
        }

        base.OnClickExecute();

        if(GameObject.Find("Tool_NKQ"))
        {
            NhiVR.Event ev = new NhiVR.Event(4, "Bóp bóng qua NKQ", 7, "Bóp", "E4");
            ev.parameters = new Parameter(1, "", "");
            ev.parameters.value = result;

            ev.Execute();
        }

    }

}
