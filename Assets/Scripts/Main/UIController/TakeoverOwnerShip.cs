using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeoverOwnerShip : MonoBehaviourPun, IPunOwnershipCallbacks
{
    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        return;
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        return;
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        throw new System.NotImplementedException();
    }

    //request ownership function
    public void TakeOwnerShip()
    {
        base.photonView.RequestOwnership();
    }

    private void Awake()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}
