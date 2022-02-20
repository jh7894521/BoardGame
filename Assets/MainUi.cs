using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUi : MonoBehaviour
{
    public Sprite[] dice;
    public static int diceNum;

    public GameObject diceUi;
    public GameObject endUi;
    public TextMeshProUGUI idText;
    public TextMeshProUGUI turnPlayerText;
    private string turnPlayer;

    public static TextMeshProUGUI idTextStatic;
    public static GameObject endUiStatic;
    public Button goButton;

    public PhotonView PV;
    public static string winner;

    private void Start()
    {
        endUiStatic = endUi;
        idTextStatic = idText;
        turnPlayerText.text = PhotonNetwork.PlayerList[0].NickName;

        if(PhotonNetwork.LocalPlayer.NickName != PhotonNetwork.PlayerList[0].NickName)
        {
            goButton.interactable = false;
        }
    }

    public void ClickGoButton()
    {
        PlayerScript.go = true;
        diceNum = Random.Range(1,6);
        Debug.Log("주사위 : " + diceNum);
        diceUi.GetComponent<Image>().sprite = dice[diceNum - 1];

        goButton.interactable = false;
        PV.RPC("Turn", RpcTarget.All);
    }

    public void ClickBackButton()
    {
        SceneManager.LoadScene("Lobby");
    }


    [PunRPC]
    void Turn()
    {
        if(turnPlayerText.text == PhotonNetwork.PlayerList[0].NickName)
        {
            turnPlayerText.text = PhotonNetwork.PlayerList[1].NickName;
            if(PhotonNetwork.LocalPlayer.NickName == PhotonNetwork.PlayerList[1].NickName)
            {
                goButton.interactable = true;
            }
        }
        else
        {
            turnPlayerText.text = PhotonNetwork.PlayerList[0].NickName;
            if(PhotonNetwork.LocalPlayer.NickName == PhotonNetwork.PlayerList[0].NickName)
            {
                goButton.interactable = true;
            }
        }
    }
}