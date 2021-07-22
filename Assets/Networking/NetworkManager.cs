using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    public static NetworkManager instance;
    public GameObject connectedPlayerPrefab;
    public GameObject nonConnectedPlayerPrefab;
    public byte maxPlayers = 4;
    public string gameVersion = "1.0";

    // The reference to our current player object
    public GameObject currentPlayer;

    Dictionary<string, RoomInfo> roomList = new Dictionary<string, RoomInfo>();
    public string currentRoomName = string.Empty;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        ConnectToMasterServer();
    }

    public void ConnectToMasterServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined Lobby");
        currentRoomName = "In lobby";
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }
        currentPlayer = Instantiate(nonConnectedPlayerPrefab, new Vector3(0, 2f, 0), Quaternion.identity);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to master");
        JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        this.roomList.Clear();
        foreach(RoomInfo info in roomList)
        {
            this.roomList.Add(info.Name, info);
        }
        Debug.Log("Updated room count: Found " + roomList.Count + " rooms!");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room");
        currentRoomName = PhotonNetwork.CurrentRoom.Name;
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }
        currentPlayer = SpawnPlayer();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join room " + returnCode + " " + message); ;
        CreateOrJoinRoom();
    }

    public void CreateOrJoinRoom()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
            Debug.Log("Left lobby");
        }

        currentRoomName = GenerateRoomName;
        // If we find the room
        while(roomList.ContainsKey(currentRoomName))
        {
            currentRoomName = GenerateRoomName;
        }
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayers;
        PhotonNetwork.JoinOrCreateRoom(currentRoomName, options, TypedLobby.Default);
    }

    public void JoinRoom(string roomName)
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
            Debug.Log("Left lobby");
        }

        if (roomList.ContainsKey(roomName))
            PhotonNetwork.JoinRoom(roomName);
        else
            Debug.LogError("Failed to join room " + roomName + ". Room does not exist!");
    }

    public void JoinRoom(InputField input)
    {
        JoinRoom(input.text);
        input.text = string.Empty;
    }

    public void JoinRandomRoom()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
            Debug.Log("Left lobby");
        }

        if (roomList.Count > 0)
            PhotonNetwork.JoinRandomRoom();
        else
            CreateOrJoinRoom();
    }

    public void JoinLobby()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            Debug.Log("Left room " + currentRoomName);
        }
        PhotonNetwork.JoinLobby();
    }

    public GameObject SpawnPlayer()
    {
        return PhotonNetwork.Instantiate(this.connectedPlayerPrefab.name, new Vector3(0f, 2f, 0f), Quaternion.identity, 0);
    }

    public string GenerateRoomName
    {
        get
        {
            string roomName = string.Empty;
            for (int i = 0; i < 6; i++)
            {
                int number = Random.Range(0, 9);
                roomName += number;
            }
            return roomName;
        }
    }
}
