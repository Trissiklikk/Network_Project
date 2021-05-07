using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Realtime;
public class LobbyManager : PunBehaviour
{
    public string characterPrefName;
    public Transform spawnPoint;

    private GameObject ownerCharacter;
    public Text dubugForPlayer;
    public Text roomNameInput;
    public Text playerNameInput;
    public GameObject lobbyUI;
    private void Start()
    {

        if (PhotonNetwork.connected == false)
        {
            PhotonNetwork.ConnectUsingSettings("1.0");
            //PhotonNetwork.playerName = playerName;
        }


    }

    public void OnGUI()
    {
        if (PhotonNetwork.insideLobby)
        {
            for (int i = 0; i < PhotonNetwork.GetRoomList().Length; i++)
            {
                RoomInfo[] rooms = PhotonNetwork.GetRoomList();
                if (GUILayout.Button("JoinRoom : " + rooms[i].Name))
                {
                    PhotonNetwork.JoinRoom(rooms[i].Name);
                }
            }
        }
        else if (PhotonNetwork.inRoom)
        {
            if (ownerCharacter == null)
            {
                if (GUILayout.Button("SpawnCharacter"))
                {
                    ownerCharacter = PhotonNetwork.Instantiate(characterPrefName, spawnPoint.position, spawnPoint.rotation, 0);
                }
            }

        }

    }

    public void Update()
    {

        dubugForPlayer.text = PhotonNetwork.connectionState.ToString();





    }



    public override void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log("Player : " + player.NickName + " is connected");
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        Debug.Log("Player : " + player.NickName + " is disconnected");
    }

    public void ButtonCreateJoin()
    {

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        PhotonNetwork.JoinOrCreateRoom(roomNameInput.text, roomOptions, TypedLobby.Default);
        lobbyUI.SetActive(false);




    }


}

