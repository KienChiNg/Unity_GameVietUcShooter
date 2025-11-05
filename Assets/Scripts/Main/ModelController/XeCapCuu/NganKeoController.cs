using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NganKeoController : MonoBehaviour, IPunObservable
{
    Vector3 close;
    Vector3 open;
    public bool isClose;
    public GameObject[] tools;
    // Start is called before the first frame update
    Vector3 end = new Vector3();
    public GameObject popupLabel;
    public AudioSource audioFile;
  

    void Start()
    {
        //init parameters
        isClose = true;
        end = transform.position;
        close = transform.position;
        open = transform.position + transform.rotation * Vector3.forward * 2.4f;
    }

    private void Update()
    {
        //moving game object when END is updated
        if (Vector3.Distance(transform.position, end) > 0.001)
        {
            transform.position = Vector3.Lerp(transform.position, end, 0.2f);
        }
    }

    private void OnMouseUp()
    {
        if(!UIEvent.instance.IsPointerOverUIObject())
        {
            //if local player is owner, execute open function
            //else request ownership of current owner
            if( GetComponent<PhotonView>().Owner == PhotonNetwork.LocalPlayer)
            {
                OpenNganKeo();
     
            } else
            {
                if (GetComponent<NganKeoOwnerShip>())
                {
                    GetComponent<NganKeoOwnerShip>().RequestOwnershipOfTool();
                }
            }
        }
    }

    public void OpenNganKeo()
    {
        //open the drawer if it is closed
        //close the drawer if it is opened
        if (isClose)
        {
            end = open;
            isClose = false;
            GetComponent<HighlightAndLabel>().displayLabel = false;
        }
        else
        {
            end = close;
            GetComponent<HighlightAndLabel>().displayLabel = true;
            isClose = true;
        }

        //close popup
        popupLabel.SetActive(false);

        //play audio
        audioFile.Play();

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            //send current drawer's position
            stream.SendNext(end);

            //send state of preparing tools
            for(int i = 0; i < tools.Length; i ++)
            {
                if(tools[i])
                {
                    if (tools[i].GetComponent<HoldingItem>())
                    {
                        stream.SendNext(tools[i].GetComponent<HoldingItem>().start);
                        stream.SendNext(tools[i].activeSelf);
                    }
                    else
                    {
                        stream.SendNext(tools[i].activeSelf);
                    }
                    
                }
                
            }

        } else
        {
            
            Vector3 new_end = (Vector3)stream.ReceiveNext();

            //receive current drawer's position
            if (new_end != end)
            {
                //move drawer
                end = new_end;
                //play audio
                audioFile.Play();
            }

            //receive state of preparing tools
            for (int i = 0; i < tools.Length; i++)
            {
                //display tools follow received state
                if(tools[i])
                {
                    if(tools[i].GetComponent<HoldingItem>())
                    {
                        bool start = (bool)stream.ReceiveNext();

                        if(start)
                        {
                            tools[i].GetComponent<HoldingItem>().SetUpPosAndStart();
                        }

                        tools[i].SetActive((bool)stream.ReceiveNext());
                    } else
                    {
                        tools[i].SetActive((bool)stream.ReceiveNext());
                    }
              
                }
            }
        }
    }
}
