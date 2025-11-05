using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float rotateSpeed = 2f;
    private float X;
    private float Y;


    void Update()
    {
     
        if (Input.GetMouseButton(1) && !UIManager.instance.actionPanelInuse)
        {
            //rotate camera with mouse right
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * rotateSpeed, -Input.GetAxis("Mouse X") * rotateSpeed, 0));
            X = transform.rotation.eulerAngles.x;
            Y = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(X, Y, 0);
        } 
    }
}
