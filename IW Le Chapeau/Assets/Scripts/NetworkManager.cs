using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class NetworkManager : MonoBehaviourPunCallbacks
{
    //instance
    public static NetworkManager instance;

    void Awake()
    {
        // if an instance already exists and it's not this one - destory us
        if (instance != null && instance != this)
            gameObject.SetActive(false);
        else
        {
            //set the instace
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //atempt to create a new room
    public void CreateRoom(string roomName)
    { 
        PhotonNetwork.CreateRoom(roomName);
    }

    //atempt to join an existing room
    public void JoinRoom (string roomName) 
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    //change the scene using Photon's system
    [PunRPC]
    public void ChangeScene(string sceneName) 
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    // called when we connect to the master server
    //enable the "Create Room" and "Join Room" buttons
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master server");
        CreateRoom("testroom");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Create room: " + PhotonNetwork.CurrentRoom.Name);
    }
}
