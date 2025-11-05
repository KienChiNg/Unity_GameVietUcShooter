using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMaskModelController : CureToolModelController
{
    public MoveToolToLocation moveTool;
    public Vector3 posNKQ;
    public Vector3 rotateNKQ;
    public GameObject mask;
    float timer0 = -1; //thời gian đếm ngược nhịp thở về 0 khi không bóp bóng
    float deltaTime0 = 2;

    public void Update()
    {
        //if(SubStateSynchronize.instance.dangBopBong)
        //{
        //    //Đếm ngược thời gian buông tay
        //    if (timer0 >= 0)
        //    {
        //        timer0 -= Time.deltaTime;
        //        //Cập nhật nhịp thở và spo2 trên điện tâm đồ
        //        ECGParameterController.instance.UpdateBreathRate(40);
        //    }
        //    else
        //    {
        //        //Cập nhật nhịp thở và spo2 trên điện tâm đồ
        //        ECGParameterController.instance.UpdateBreathRate();
        //    }
        //}
    }

    public override void DisplayTool()
    {
        base.DisplayTool();
        //move game object to Target Position
        moveTool.MoveToTarget();

        //hide oxy mask if simbaby is using oxy mask
        //SubStateSynchronize.instance.SynchronizeBopBong(true);

        //if simbaby have NKQ
        if (GameObject.Find("Tool_NKQ"))
        {
            gameObject.transform.SetParent(GameObject.Find("Tool_NKQ").transform);
            moveTool.MoveToPos(posNKQ, rotateNKQ);
            mask.SetActive(false);
        }

        //
        timer0 = -1;
    }

    public override void HideTool()
    {
        base.HideTool();
        //move game object to Previous Position
        moveTool.MoveToTable();

        //
        //SubStateSynchronize.instance.SynchronizeBopBong(false);

        //if simbaby have NKQ
        if (GameObject.Find("Tool_NKQ"))
        {
            mask.SetActive(true);
        }

        //
    }

    public override void ActivateAnimation()
    {
        base.ActivateAnimation();

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

        //inc CPR counter
        CPRCounterController.instance.BopBongCounter();

        //
        timer0 = deltaTime0;
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

        //inc CPR counter
        CPRCounterController.instance.BopBongCounter();

        //
        timer0 = deltaTime0;
    }

    public override void OnOwnershipTransferedAction()
    {
        base.OnOwnershipTransferedAction();

    }
}
