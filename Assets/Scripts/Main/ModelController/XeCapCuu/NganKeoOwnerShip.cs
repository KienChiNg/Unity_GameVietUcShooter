using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NganKeoOwnerShip : MonoBehaviourPun, IPunOwnershipCallbacks
{
    private void Awake()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != base.photonView || (targetView.Owner != null && targetView.Owner != PhotonNetwork.LocalPlayer) || (targetView.Owner == null && requestingPlayer != PhotonNetwork.LocalPlayer) )
            return;

        //Add checks here.
        if (gameObject.GetComponent<NganKeoController>())
        {
            //only transfer ownership if it's closed
            if(GetComponent<NganKeoController>().isClose == true)
            {
                base.photonView.TransferOwnership(requestingPlayer);
            }
            
        }
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        if (targetView.Owner != PhotonNetwork.LocalPlayer || targetView != base.photonView)
            return;
        //open the drawer when ownership is transfered
        if (GetComponent<NganKeoController>())
        {
            GetComponent<NganKeoController>().OpenNganKeo();
        }
    }

    //request ownership function
    public void RequestOwnershipOfTool()
    {
        base.photonView.RequestOwnership();
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        throw new System.NotImplementedException();
    }
}
