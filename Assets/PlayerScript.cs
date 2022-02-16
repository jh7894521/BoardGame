using UnityEngine;
using Photon.Pun;
using System;
using System.Linq;

public class PlayerScript : MonoBehaviour
{
    public float speed = 5f;
    private float health = 4;

    private Rigidbody characterRigidbody;
    PhotonView view;

    private string nowPosition = string.Empty;
    private Transform[] blockArr;
    public static bool go = false;
    private int substringIndex;
    private int randomBlock = 0;
    private int bombPower = 100;

    void Start()
    {
        characterRigidbody = GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();
        //blockArr = GameObject.FindGameObjectsWithTag("Block");
        blockArr = GameObject.Find("Environment").GetComponentsInChildren<Transform>();
        blockArr = blockArr.OrderBy(go => go.name).ToArray();
        
        substringIndex = 0;
        
        Debug.Log("아이디 : " + view.ViewID);
    }    

    void Update()
    {
        if (view.IsMine)
        {
            //체력이 1보다 아래면 죽음 낙사하면 죽음 > 처음 위치로
            if (health < 1 || transform.position.y < -1)
            {
                transform.position = new Vector3(-8, 0.3f, -7);
                health = 10;
            }

            //Move();

            if (go)
            {
                go = false;
                GoBlock();
            }
        }
    }

    void Move()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        // -1 ~ 1

        float fallSpeed = characterRigidbody.velocity.y; 

        Vector3 velocity = new Vector3(inputX, 0, inputZ);
        velocity *= speed;
        velocity.y = fallSpeed;
        characterRigidbody.velocity = velocity;
    }

    //블럭이동
    void GoBlock()
    {
        if (nowPosition != string.Empty)
        {
            if (nowPosition.Substring(2,2).Equals("바닥"))
            {
                substringIndex = nowPosition.IndexOf("바닥");
            }
            else if(int.Parse(nowPosition.Substring(0, substringIndex)) > 47)
            {
                MainUi.idTextStatic.text = CreateAndJoinRooms.nickNameTextStatic.text;
                MainUi.endUiStatic.SetActive(true);

                return;
            }
            //transform.position = Vector3.Slerp(transform.position, blockArr[int.Parse(nowPosition.Substring(0, substringIndex))].transform.position + new Vector3(0, 0.3f, 0), 1f);
            transform.position = Vector3.Slerp(transform.position, blockArr[int.Parse(nowPosition.Substring(0, substringIndex))-1 + MainUi.diceNum].transform.position + new Vector3(0, 0.3f, 0), 1f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.transform.tag)
        {
            case "Bomb":    //폭탄
                //randomBlock = UnityEngine.Random.Range(int.Parse(nowPosition.Substring(0, substringIndex)) - 2, int.Parse(nowPosition.Substring(0, substringIndex)) + 2);
                //transform.position = Vector3.Slerp(transform.position, blockArr[randomBlock].transform.position + new Vector3(0, 0.3f, 0), 1f);

                nowPosition = collision.transform.parent.name;
                BombMove(nowPosition);
                break;

            case "Thorn":   //가시
                health--;
                Debug.Log("체력 : " + health);

                nowPosition = collision.transform.parent.name;
                break;

            case "End":     //목표지점
                //CreateAndJoinRooms.nickNameTextStatic.text = view.GetInstanceID().ToString();
                MainUi.idTextStatic.text = CreateAndJoinRooms.nickNameTextStatic.text;
                MainUi.endUiStatic.SetActive(true);

                nowPosition = collision.transform.parent.name;
                break;
            case "Water":   //물
                nowPosition = collision.transform.name;
                WaterMove(nowPosition);
                break;
            default:
                nowPosition = collision.transform.name;
                break;
        }
    }

    //물 흐름
    void WaterMove(string name)
    {
        float x = blockArr[int.Parse(name.Substring(0, 2)) - 1].transform.position.x - blockArr[int.Parse(name.Substring(0, 2)) - 2].transform.position.x;
        float z = blockArr[int.Parse(name.Substring(0, 2)) - 1].transform.position.z - blockArr[int.Parse(name.Substring(0, 2)) - 2].transform.position.z;
        if (x == 0)
        {
            if (z > 0)
            {
                characterRigidbody.velocity = new Vector3(0, 1.5f, -0.1f);
            }
            else
            {
                characterRigidbody.velocity = new Vector3(0, 1.5f, 0.1f);
            }
        }
        else if (z == 0)
        {
            if (x > 0)
            {
                characterRigidbody.velocity = new Vector3(-0.1f, 1.5f, 0);
            }
            else
            {
                characterRigidbody.velocity = new Vector3(0.1f, 1.5f, 0);
            }
        }
    }

    //폭탄
    void BombMove(string name)
    {
        float x = blockArr[int.Parse(name.Substring(0, 2)) - 1].transform.position.x - blockArr[int.Parse(name.Substring(0, 2)) - 2].transform.position.x;
        float z = blockArr[int.Parse(name.Substring(0, 2)) - 1].transform.position.z - blockArr[int.Parse(name.Substring(0, 2)) - 2].transform.position.z;
        if (x == 0)
        {
            if (z > 0)  //가로
            {
                characterRigidbody.AddForce(new Vector3(0, 1f, -1f) * bombPower);
            }
            else
            {
                characterRigidbody.AddForce(new Vector3(0, 1f, 1f) * bombPower);
            }
        }
        else if (z == 0)    //세로
        {
            if (x > 0)
            {
                characterRigidbody.AddForce(new Vector3(-1f, 1f, 0) * bombPower);
            }
            else
            {
                //아직안씀
                characterRigidbody.AddForce(new Vector3(1f, 1f, 0) * bombPower);
            }
        }
    }
}