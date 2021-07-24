using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LobbySettings : MonoBehaviour
{

    public List<string> whosReady = new List<string>();
    public PhotonView photonView;
    public UnityEvent<string> OnReady, OnUnready;
    public UnityEvent OnAllReady, OnNotAllReady;

    public void ToggleReady()
    {
        if(whosReady.Contains(PhotonNetwork.LocalPlayer.UserId))
        {
            Unready();
        } else
        {
            ReadyUp();
        }
    }

    public void ReadyUp()
    {
        photonView.RPC("RPC_Ready", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.UserId);
    }

    public void Unready()
    {
        photonView.RPC("RPC_Unready", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.UserId);
    }

    public void RPC_Ready(string id)
    {
        if(!whosReady.Contains(id))
        {
            whosReady.Add(id);
            if(id.Equals(PhotonNetwork.LocalPlayer.UserId))
                OnReady?.Invoke(id);
            CheckIfAllReady();
        }
    }

    public void RPC_Unready(string id)
    {
        if(whosReady.Contains(id))
        {
            whosReady.Remove(id);
            if (id.Equals(PhotonNetwork.LocalPlayer.UserId))
                OnUnready?.Invoke(id);
            CheckIfAllReady();
        }
    }
    
    public void CheckIfAllReady()
    {
        if(whosReady.Count == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                OnAllReady?.Invoke();
            } else
            {
                OnNotAllReady?.Invoke();
            }
        }
    }

}
