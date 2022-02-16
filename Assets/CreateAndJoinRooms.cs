using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    public GameObject NickNameUi;
    public GameObject LobbyUi;

    public TMP_InputField nickNameInput;

    public TextMeshProUGUI playerCount;
    public TextMeshProUGUI nickNameText;
    public static TextMeshProUGUI nickNameTextStatic;

    private void Awake()
    {
        nickNameTextStatic = nickNameText;
    }

    private void Update()
    {
        playerCount.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + " Lobby / " + PhotonNetwork.CountOfPlayers + " Connect";
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public void ConnectLobby()
    {
        PhotonNetwork.LocalPlayer.NickName = nickNameInput.text;
        nickNameText.text = PhotonNetwork.LocalPlayer.NickName;
        NickNameUi.SetActive(false);
        LobbyUi.SetActive(true);
    }

    public void ClickLobbyBackButton()
    {
        LobbyUi.SetActive(false);
        NickNameUi.SetActive(true);
    }

    public void ClickBackButton()
    {
        Application.Quit();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
