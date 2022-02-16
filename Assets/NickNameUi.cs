using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NickNameUi : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickBackButton()
    {
        Application.Quit();
    }

    public void ClickConnectButton()
    {
        SceneManager.LoadScene("Lobby");
    }
}