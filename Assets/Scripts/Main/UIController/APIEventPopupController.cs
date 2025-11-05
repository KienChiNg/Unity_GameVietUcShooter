using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class APIEventPopupController : MonoBehaviour
{
    [Header("Popup Effect")]
    public UIEffectComponent openEffect;
    public UIEffectComponent closeEffect;
    [Header("Event")]
    public NhiVR.Event action;
    public GameObject executeButton;
    public UnityAction executeAction;
    public Button closeBtn;
    

    // Start is called before the first frame update
    public virtual void Start()
    {
        gameObject.SetActive(false);
        executeAction = () => { OnClickExecute(); };
        
        //set close button listener
        closeBtn.onClick.AddListener(() => { ClosePopup(); });
    }

    public virtual bool OpenPopup(NhiVR.Event action)
    {
        if(openEffect.DisplayEffect(gameObject, true))
        {
            //display popup
            gameObject.SetActive(true);

            UIManager.instance.actionPanelInuse = true;

            //set new event 
            if (action != null)
            {
                this.action = action;
            }

            //remove and add execute button listener again to update new event
            executeButton.GetComponent<Button>().onClick.RemoveListener(executeAction);
            executeButton.GetComponent<Button>().onClick.AddListener(executeAction);

            return true;
        }

        return false;
    }

    public virtual void ClosePopup()
    {
        //close popup
        if (closeEffect.DisplayEffect(gameObject, false))
        {
            UIManager.instance.actionPanelInuse = false;
        }
    }

    public virtual void OnClickExecute()
    {
        //execute event
        action.parameters.value_str = "TRUE";
        action.Execute();
    }
}
