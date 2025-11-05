using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class NavMeshController : MonoBehaviour, IPunObservable
{
    public NavMeshAgent navMesh;
    public Vector3 target;
    Vector3 player_pos_at_bed = new Vector3(1.2f, 0, -4.4f);
    Vector3 player_pos_at_table = new Vector3(-7.5f, 0, -0.5f);
    Vector3 player_pos_at_board = new Vector3(5.5f, 0, -4.5f);
    Vector3 player_pos_at_cabinet = new Vector3(-10, 0, 5);
    Vector3 player_pos_at_door = new Vector3(-4, 0, 11);
    Vector3 player_pos_at_maysocdien = new Vector3(2.45f, 0, -6.2f);
    Vector3 player_pos_at_maytiemdien = new Vector3(3.85f, 0, -6.15f);
    Vector3 player_pos_at_mayhutdai = new Vector3(-10.42f, 0, -7.74f);
    string currentPos = "";
    Quaternion targetQuaternion = new Quaternion();
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {

        if (!navMesh.pathPending)
        {
            if (navMesh.remainingDistance <= navMesh.stoppingDistance)
            {
                if (!navMesh.hasPath || navMesh.velocity.sqrMagnitude == 0f)
                {
                    // Done
                    navMesh.GetComponent<Animator>().SetBool("isWalking", false);
                    UpdateQuaternion();
                }
            } else
            {
                if (navMesh.velocity.sqrMagnitude != 0f)
                {
                    // Done
                    navMesh.GetComponent<Animator>().SetBool("isWalking", true);
                }
                
            }
        }

    }

    void UpdateQuaternion()
    {

        if(Quaternion.Angle(transform.rotation, targetQuaternion) > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetQuaternion, 0.1f);
        }

    }

    public void MoveTo(string destination)
    {
        currentPos = destination;
        float angle = 0;
        //

        switch (destination)
        {
            case "bed":
                navMesh.SetDestination(player_pos_at_bed);
                angle = -103;
                break;
            case "drug_cabinet":
                navMesh.SetDestination(player_pos_at_cabinet);
                angle = -156;
                break;
            case "table":
                navMesh.SetDestination(player_pos_at_table);
                angle = -130;
                break;
            case "board":
                navMesh.SetDestination(player_pos_at_board);
                angle = 141;
                break;
            case "maysocdien":
                navMesh.SetDestination(player_pos_at_maysocdien);
                angle = -141;
                break;
            case "maytiemdien":
                navMesh.SetDestination(player_pos_at_maytiemdien);
                angle = -161;
                break;
            case "mayhutdai":
                navMesh.SetDestination(player_pos_at_mayhutdai);
                angle = -282;
                break;
            case "door":
                navMesh.SetDestination(player_pos_at_door);
                angle = 181;
                break;
        }

        targetQuaternion = Quaternion.AngleAxis(angle, Vector3.up);
    }

    //on synchronize
    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentPos);
        }
        else
        {
            string location = (string)stream.ReceiveNext();
            if(location != currentPos)
            {
                currentPos = location;
                MoveTo(currentPos);
            }
        }
    }
}
