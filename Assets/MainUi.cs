using System.Collections;
using System.Collections.Generic;
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

    public static TextMeshProUGUI idTextStatic;
    public static GameObject endUiStatic;

    private void Start()
    {
        endUiStatic = endUi;
        idTextStatic = idText;
    }

    public void ClickGoButton()
    {
        Player.go = true;
        diceNum = Random.Range(1,6);
        Debug.Log("주사위 : " + diceNum);
        diceUi.GetComponent<Image>().sprite = dice[diceNum - 1];
    }

    public void ClickBackButton()
    {
        SceneManager.LoadScene("Lobby");
    }
}