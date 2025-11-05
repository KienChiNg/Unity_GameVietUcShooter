using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureToolModelController : MonoBehaviourPun, IPunOwnershipCallbacks, IPunObservable
{
    public NhiVR.Event action;
    //is model available
    public bool inUse = false;
    public bool displayStatus = false;
    public bool isSynchronizeDisplay = true;
    //move camera
    public camlerp cam;
    //Audio Src
    public AudioSource audioSrc;
    //animator
    [Header("Animator")]
    public bool isDisplayWithAnimator = true;
    public Animator[] animators;
    [Header("Display parts")]
    public bool isDisplayWithPart = true;
    public GameObject[] parts;
    //UI controller
    [Header("UI Controller")]
    public GameObject popup;

    //when a player request ownership of tool
    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != base.photonView || (targetView.Owner != null && targetView.Owner != PhotonNetwork.LocalPlayer) || (targetView.Owner == null && requestingPlayer != PhotonNetwork.LocalPlayer))
            return;

        OnOwnershipRequestAction(requestingPlayer);
    }

    //when an ownership of tool is transfered
    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        if (targetView.Owner != PhotonNetwork.LocalPlayer || targetView != base.photonView)
            return;
        
        OnOwnershipTransferedAction();
    }

    //action will be executed when a player request ownership of tool
    public virtual void OnOwnershipRequestAction(Player reqPlayer)
    {
        if(!inUse)
        {
            photonView.TransferOwnership(reqPlayer);
        }
    }

    //action will be executed when an ownership of tool is transfered
    public virtual void OnOwnershipTransferedAction()
    {
        bool successOpenPopup = true;
        //activate UI
        if(popup)
        {
            successOpenPopup = popup.GetComponent<APIEventPopupController>().OpenPopup(action);
        }

        if(successOpenPopup)
        {
            //move cam to em be
            if (cam)
            {
                cam.moveToEbe();
            }

            //display tool
            DisplayTool();
        }

    }

    //send request to get ownership
    public void RequestOwnershipOfTool()
    {
        if (!photonView.IsOwnerActive || photonView.OwnerActorNr == 0 || photonView.Owner == PhotonNetwork.LocalPlayer)
        {
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }
        else
        {
            base.photonView.RequestOwnership();
        }

    }

    //Display model
    public virtual void DisplayTool()
    {
        inUse = true;
        displayStatus = true;
        
        //run Display animation
        if(isDisplayWithAnimator)
        {
            for (int i = 0; i < animators.Length; i++)
            {
                animators[i].SetBool("on", true);
            }
        }

        //enable invisible parts
        if (isDisplayWithPart)
        {
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i].SetActive(true);
            }
        }

        

    }

    //Hide model
    public virtual void HideTool()
    {
        inUse = false;
        displayStatus = false;

        //run Display animation
        if (isDisplayWithAnimator)
        {
            for (int i = 0; i < animators.Length; i++)
            {
                animators[i].SetBool("on", false);
            }
        }

        //disable visible parts
        if (isDisplayWithPart)
        {
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i].SetActive(false);
            }
        }

    }

    //animation of tool in owner's scene
    public virtual void ActivateAnimation()
    {
        //play audio sound if exists
        if (audioSrc)
        {
            audioSrc.Play();
        }
        //activate animation if exists
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].SetTrigger("active");
        }
    }

    //animation of tool in other player's scene
    public virtual void ActivateOtherPlayerAnimation()
    {
        //play audio sound if exists
        if (audioSrc)
        {
            audioSrc.Play();
        }
        //activate animation if exists
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].SetTrigger("active");
        }

    }

    //on synchronize
    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (isSynchronizeDisplay)
            {
                stream.SendNext(displayStatus);
            }
        }
        else
        {
            if (isSynchronizeDisplay)
            {
                if (stream.PeekNext() is bool)
                {
                    bool status = (bool)stream.ReceiveNext();
                    switch (status)
                    {
                        case true:
                            if(displayStatus == false)
                            {
                                DisplayTool();
                                displayStatus = true;
                            }
                            break;
                        case false:
                            if (displayStatus == true)
                            {
                                HideTool();
                                displayStatus = false;
                            }
                            break;
                    }

                }
            }
        }
    }

    //on mouse button up
    private void OnMouseUp()
    {
        if(!UIEvent.instance.IsPointerOverUIObject() && !UIManager.instance.actionPanelInuse)
        {
            RequestOwnershipOfTool();
        }
    }

    public virtual void Awake()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        throw new System.NotImplementedException();
    }
}
