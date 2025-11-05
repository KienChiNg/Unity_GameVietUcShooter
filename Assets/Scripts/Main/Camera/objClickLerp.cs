using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objClickLerp : MonoBehaviour
{
    public Transform pos;
    public camlerp cam;
    public string target;
    NavMeshController controller = null;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    //enable highlight box when mouse in
    private void OnMouseEnter()
    {
        if(!UIManager.instance.actionPanelInuse && !UIEvent.instance.IsPointerOverUIObject())
        {
            GetComponent<highlight>().enabled = true;
        }
       
    }

    //disable highlight box when mouse in
    private void OnMouseExit()
    {
        GetComponent<highlight>().enabled = false;
    }


    private void OnMouseDown()
    {
        if(!UIManager.instance.actionPanelInuse && !UIEvent.instance.IsPointerOverUIObject())
        {
            //move to clicked box
            cam.move(pos, gameObject);

            //if(controller != null)
            //{
            //    controller.MoveTo(target);
            //} else
            //{
            //    controller = GameObject.Find("Player").GetComponent<NavMeshController>();
            //    if(controller)
            //    {
            //        controller.MoveTo(target);
            //    }

            //}

        }
         
    }
}
