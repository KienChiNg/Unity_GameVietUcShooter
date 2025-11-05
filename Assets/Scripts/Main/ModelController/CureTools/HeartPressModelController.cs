using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class HeartPressModelController : CureToolModelController
{

    public override void ActivateAnimation()
    {
        
        base.ActivateAnimation();
        //trigger animation of simbaby
        SimbabyController.instance.ani.SetTrigger(gameObject.name);
        if(SimbabyController.instance.diencucMonitor)
        {
            SimbabyController.instance.diencucMonitor.SetTrigger(gameObject.name);
        }

        if (SimbabyController.instance.dayMonitorAnimator)
        {
            SimbabyController.instance.dayMonitorAnimator.SetTrigger(gameObject.name);
        }

        //CPR inc press counter
        CPRCounterController.instance.PressCounter();


    }

    public override void ActivateOtherPlayerAnimation()
    {
        base.ActivateOtherPlayerAnimation();
        //trigger animation of simbaby
        SimbabyController.instance.ani.SetTrigger(gameObject.name);
        if (SimbabyController.instance.diencucMonitor)
        {
            SimbabyController.instance.diencucMonitor.SetTrigger(gameObject.name);
        }

        if (SimbabyController.instance.dayMonitorAnimator)
        {
            SimbabyController.instance.dayMonitorAnimator.SetTrigger(gameObject.name);
        }

        //CPR inc press counter
        CPRCounterController.instance.PressCounter();


    }

    public override void OnOwnershipRequestAction(Player reqPlayer)
    {
        if(!base.photonView.IsOwnerActive)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        
    }

    public override void HideTool()
    {
        base.HideTool();
        PhotonNetwork.Destroy(gameObject);
    }

    public void OnEnable()
    {
        CPRCounterController.instance.OpenPopup();
        gameObject.name = gameObject.name.Replace("(Clone)", "");
      
        GameObject bantayPos = GameObject.Find("BantayPosition");
        if (bantayPos)
        {
            gameObject.transform.position = bantayPos.transform.position;
            gameObject.transform.localScale = bantayPos.transform.localScale;
            gameObject.transform.rotation = bantayPos.transform.rotation;
        }
    }

    public void OnDisable()
    {
        CPRCounterController.instance.ClosePopup();
    }
}
