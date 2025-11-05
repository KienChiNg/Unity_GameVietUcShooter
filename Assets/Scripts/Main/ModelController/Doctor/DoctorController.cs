using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorController : MonoBehaviour
{
    public static DoctorController instance;
    public Animator ani;
    public camlerp camlerp;
    public Transform cambacsi;
    public Transform monitor;
    public bool isBabyOnBed = true;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    public void VoLung()
    {
        CancelInvoke();
        monitor.gameObject.SetActive(false);
        DisplayModel();
        if (isBabyOnBed)
        {
            NhacKhoiGiuong();
        }
        //
        //
        ani.SetBool("latsap", true);
        SimbabyController.instance.ani.SetBool("latsap", true);

        ani.SetTrigger("volung");
        SimbabyController.instance.ani.SetTrigger("volung");

    }

    public void AnNguc()
    {
        CancelInvoke();
        DisplayModel();
        if(isBabyOnBed)
        {
            NhacKhoiGiuong();
        }
        //
        //
        ani.SetBool("latsap", false);
        SimbabyController.instance.ani.SetBool("latsap", false);

        ani.SetTrigger("annguc");
        SimbabyController.instance.ani.SetTrigger("annguc");
        //
    }

    public bool DatLenGiuong()
    {
        if(isBabyOnBed == false)
        {
            isBabyOnBed = true;
            camlerp.moveToEbe();
            //
            ani.SetTrigger("changePose");
            SimbabyController.instance.ani.SetTrigger("changePose");
            //
            ani.SetInteger("datlengiuong", 1);
            SimbabyController.instance.ani.SetInteger("datlengiuong", 1);
            //
            Invoke("HideModel", 5.5f);
            return false;
        } else
        {
            return true;
        }
    }

    public void NhacKhoiGiuong()
    {
        isBabyOnBed = false;
        camlerp.move(cambacsi, null);
        //
        ani.SetTrigger("changePose");
        SimbabyController.instance.ani.SetTrigger("changePose");
        ani.SetInteger("datlengiuong", 0);
        SimbabyController.instance.ani.SetInteger("datlengiuong", 0);
    }

    public void DisplayModel()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    public void HideModel()
    {
        gameObject.transform.localScale = new Vector3(0, 0, 0);
        monitor.gameObject.SetActive(true);
    }
}
