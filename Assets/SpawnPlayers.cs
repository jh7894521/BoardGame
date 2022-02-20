using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;

    public float minX;
    public float minZ;
    public float maxX;
    public float maxZ;

    private void Start()
    {
        Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), 1.5f, Random.Range(minZ, maxZ));
        //Vector3 randomPosition = new Vector3(-8, 0.3f, -7);
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity).name = PhotonNetwork.PlayerList[0].NickName;
        }
        else
        {
            PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity).name = PhotonNetwork.PlayerList[1].NickName;
        }
    }
}