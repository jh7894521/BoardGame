using Photon.Pun;
using Photon.Realtime;
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

    public GameObject nickNameUi;
    public GameObject lobbyUi;
    public GameObject roomUi;

    public TMP_InputField nickNameInput;
    public TMP_InputField roomCreateText;

    public TextMeshProUGUI playerCount;
    public TextMeshProUGUI nickNameText;
    public TextMeshProUGUI roomTitleText;
    public TextMeshProUGUI playerListText;
    public static TextMeshProUGUI nickNameTextStatic;

    public Button[] CellButton;
    public Button PreviousButton;
    public Button NextButton;

    //��
    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;

    //�� ���� ����
    //public Image roomMasterImage;
    //public Image roomPlayerImage;
    public TextMeshProUGUI roomMasterText;
    public TextMeshProUGUI roomPlayerText;

    public PhotonView PV;

    //�渮��Ʈ ����
    public void MyListClick(int num)
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else PhotonNetwork.JoinRoom(myList[multiple + num].Name);
        MyListRenewal();
    }

    void MyListRenewal()
    {
        // �ִ�������
        maxPage = (myList.Count % CellButton.Length == 0) ? myList.Count / CellButton.Length : myList.Count / CellButton.Length + 1;

        // ����, ������ư
        PreviousButton.interactable = (currentPage <= 1) ? false : true;
        NextButton.interactable = (currentPage >= maxPage) ? false : true;

        // �������� �´� ����Ʈ ����
        multiple = (currentPage - 1) * CellButton.Length;
        for (int i = 0; i < CellButton.Length; i++)
        {
            CellButton[i].interactable = (multiple + i < myList.Count) ? true : false;
            CellButton[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
            CellButton[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (multiple + i < myList.Count) ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
        }
        MyListRenewal();
    }
    //�� ����Ʈ

    //��    
    public void CreateRoom() => PhotonNetwork.CreateRoom(roomCreateText.text == "" ? "Room" + Random.Range(0, 100) : roomCreateText.text, new RoomOptions { MaxPlayers = 2 });

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnJoinedRoom()
    {
        roomUi.SetActive(true);
        lobbyUi.SetActive(false);
        RoomRenewal();
    }

    public override void OnCreateRoomFailed(short returnCode, string message) { roomCreateText.text = ""; CreateRoom(); }

    public override void OnJoinRandomFailed(short returnCode, string message) { roomCreateText.text = ""; CreateRoom(); }

    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RoomRenewal();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomRenewal();
    }    

    void RoomRenewal()
    {
        playerListText.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playerListText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", ");
        }

        roomMasterText.text = PhotonNetwork.PlayerList[0].NickName;
        if (PhotonNetwork.PlayerList.Length > 1)
        {
            roomPlayerText.text = PhotonNetwork.PlayerList[1].NickName;
        }

        roomTitleText.text = PhotonNetwork.CurrentRoom.Name + " / " + PhotonNetwork.CurrentRoom.PlayerCount + "�� / " + PhotonNetwork.CurrentRoom.MaxPlayers + "�ִ�";
    }

    public void ClickRoomBackButton()
    {
        lobbyUi.SetActive(true);
        roomUi.SetActive(false);
    }

    public void ClickGameStart()
    {
        PV.RPC("EnterGame", RpcTarget.All);
    }

    [PunRPC]
    void EnterGame()
    {
        //if (nickNameText.Equals(roomMasterText) && PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        //{
        PhotonNetwork.LoadLevel("Game");
        //}
    }
    //��

    private void Awake()
    {
        nickNameTextStatic = nickNameText;
    }

    private void Update()
    {
        playerCount.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + " Lobby / " + PhotonNetwork.CountOfPlayers + " Connect";
    }

    /*
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }
    */

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public void ConnectLobby()
    {
        PhotonNetwork.LocalPlayer.NickName = nickNameInput.text;
        nickNameText.text = PhotonNetwork.LocalPlayer.NickName;
        nickNameUi.SetActive(false);
        lobbyUi.SetActive(true);
        myList.Clear();
    }

    public void ClickLobbyBackButton()
    {
        lobbyUi.SetActive(false);
        nickNameUi.SetActive(true);
    }

    public void ClickBackButton()
    {
        Application.Quit();
    }
}