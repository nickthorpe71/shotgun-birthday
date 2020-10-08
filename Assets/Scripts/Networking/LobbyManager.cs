using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{

    [SerializeField] GameObject findGamePanel;
    [SerializeField] GameObject searchPanel;
    [SerializeField] GameObject connectingText;

    private string playerClass = "Gun";
    private byte maxPlayers = 17;

    void Start()
    {
        searchPanel.SetActive(false);
        findGamePanel.SetActive(false);
        connectingText.SetActive(true);

        // Connect to Photon server
        PhotonNetwork.ConnectUsingSettings(); 
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("We are connected to Photon on " + PhotonNetwork.CloudRegion + " Server");
        PhotonNetwork.AutomaticallySyncScene = true;
        findGamePanel.SetActive(true);
        connectingText.SetActive(false);

    }

    public void SpawnPlayerGun()
    {
        playerClass = "Gun";
        FindMatch();
    }

    public void SpawnPlayerSword()
    {
        playerClass = "Sword";
        FindMatch();
    }

    public void FindMatch()
    {
        searchPanel.SetActive(true);
        findGamePanel.SetActive(false);

        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Searching for a game");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Unable to find a room - creating new room");
        MakeRoom();
    }

    void MakeRoom()
    {
        int randomRoomName = Random.Range(0, 5000);
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = maxPlayers
        };
        PhotonNetwork.CreateRoom("room_" + randomRoomName, roomOptions);
        Debug.Log("New room created");

        PhotonNetwork.LoadLevel(1);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New player joined - Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public void StopSearch()
    {
        findGamePanel.SetActive(true);
        searchPanel.SetActive(false);

        PhotonNetwork.LeaveRoom();
        Debug.Log("Stopped search, back to menu");
    }
}
