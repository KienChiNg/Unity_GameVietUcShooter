using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToolToLocation : MonoBehaviour
{
    [Header("Target coordinator")] 
    public Vector3 posTarget;
    public Vector3 rotateTarget;
    [Header("First coordinator")] 
    public Vector3 posTable;
    public Vector3 rotateTable;
    public Transform table;

    Quaternion endQuaternion;
    Vector3 endPos;
    // Start is called before the first frame update
    void Start()
    {
        //init end pos and quaternion 
        endQuaternion = gameObject.transform.localRotation;
        endPos = gameObject.transform.localPosition;
    }

    private void Update()
    {
        //lerp game object position and quaternion to NEW position and quaternion
        if(Vector3.Distance(gameObject.transform.localPosition, endPos) >= 0.001f)
        {
            gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, endPos, 0.2f);
        }
        
        if(Quaternion.Angle(transform.localRotation, endQuaternion) >= 0.001f)
        {
            gameObject.transform.localRotation = Quaternion.Lerp(gameObject.transform.localRotation, endQuaternion, 0.2f);
        }
    }

    //set NEW position and quaternion with TARGET COORDINATOR
    public void MoveToTarget()
    {
        gameObject.transform.SetParent(table.transform);
        endQuaternion = Quaternion.Euler(rotateTarget.x, rotateTarget.y, rotateTarget.z);
        endPos = posTarget;
    }

    //set NEW position and quaternion with TABLE COORDINATOR
    public void MoveToTable()
    {
        gameObject.transform.SetParent(table.transform);
        endQuaternion = Quaternion.Euler(rotateTable.x, rotateTable.y, rotateTable.z);
        endPos = posTable;
    }

    public void MoveToPos(Vector3 newPos, Vector3 newRotate)
    {
        endQuaternion = Quaternion.Euler(newRotate.x, newRotate.y, newRotate.z);
        endPos = newPos;
    }
}
