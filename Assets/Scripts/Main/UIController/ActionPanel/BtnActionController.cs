using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnActionController : MonoBehaviour
{
    public NhiVR.Event action;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(() => { OnClick(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstanceModelEpTim()
    {
        GameObject prefab = Resources.Load<GameObject>("Model/E5");
        if (SymbabyCureEventController.instance.OpenPopup(action))
        {
            GameObject tools = PhotonNetwork.Instantiate("Model/" + action.eventcode, prefab.transform.position, prefab.transform.rotation);
            tools.name = action.eventcode;
            GameObject bantayPos = GameObject.Find("BantayPosition");
            if (bantayPos)
            {
                tools.transform.position = bantayPos.transform.position;
                tools.transform.localScale = bantayPos.transform.localScale;
                tools.transform.rotation = bantayPos.transform.rotation;
            }

            UIManager.instance.actionPanelInuse = true;
            //
        }
    }

    private void OnClick()
    {
        if(GameObject.Find(action.eventcode) || GameObject.Find(action.eventcode + "(Clone)"))
        {
            //SelectLocationController.instance.initSelectLocationTab(action);
            //PanelActionController.instance.openTab(2);
            //GameObject.Find(action.eventcode).GetComponent<ToolController>().displayTool(true);
            
        } else
        {
            GameObject prefab = Resources.Load<GameObject>("Model/" + action.eventcode);

            if(prefab != null)
            {
                if (DoctorController.instance)
                {
                    if(DoctorController.instance.DatLenGiuong())
                    {
                        //
                        Invoke("InstanceModelEpTim", 0);
                    } else
                    {
                        Invoke("InstanceModelEpTim", 3f);
                    }
                    
                } else
                {
                    Invoke("InstanceModelEpTim", 0);
                }
                
                
            } else
            {
                

                //
            }
            
        }

    }
}
