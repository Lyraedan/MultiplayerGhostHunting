using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Billboard : MonoBehaviourPunCallbacks
{

    public Canvas canvas;
    public Text lobbyCodeDisplay;
    public UnityEvent joinedLobby, joinedRoom;

    private void Update()
    {
        if(canvas.worldCamera == null)
        {
            canvas.worldCamera = Camera.main;
        }
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        lobbyCodeDisplay.text = "In lobby";
        joinedLobby?.Invoke();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        lobbyCodeDisplay.text = NetworkManager.instance.currentRoomName;
        joinedRoom?.Invoke();
    }
}
