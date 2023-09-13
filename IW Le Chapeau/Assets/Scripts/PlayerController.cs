using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEditor.Experimental.GraphView;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    [HideInInspector]
    public int id;

    [Header("Info")]
    public float movespeed;
    public float jumpforce;
    public GameObject hatObject;

    [HideInInspector]
    public float curHatTime;

    [Header("Components")]
    public Rigidbody rig;
    public Player photonPlayer;
    public Material red;
    public Material green;
    public Material blue;
    public Material purple;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Space))
            TryJump();

        // the host will check if the player has won
        if (PhotonNetwork.IsMasterClient)
        {
            if (curHatTime >= GameManager.instance.timeToWin && !GameManager.instance.gameEnded)
            { 
                GameManager.instance.gameEnded = true;
                GameManager.instance.photonView.RPC("WinGame", RpcTarget.All, id);
            }
        }

        // track the amount of time we're wearing the hat
        if (hatObject.activeInHierarchy)
            curHatTime += Time.deltaTime;
    }

    // called when the player object is instanizted
    [PunRPC]
    public void Initialize(Player player)
    { 
        photonPlayer = player;
        id = player.ActorNumber;

        GameManager.instance.players[id - 1] = this;

        //give the first player the hat

        // if this isn't our local player, disable as that's 
        // controlled by the user and synced to all other clients
        if(!photonView.IsMine)
            rig.isKinematic = true;

        // give the first player the hat
        if (id == 1)
            GameManager.instance.GiveHat(id, true);

        // gives player a color
        setColor(id);
    }


    [PunRPC]
    public void setColor(int c)
    { 
        if(c == 1)
            this.transform.gameObject.GetComponent<Renderer>().material = red;
        else if(c == 2)
            this.transform.gameObject.GetComponent<Renderer>().material = green;
        else if(c == 3)
            this.transform.gameObject.GetComponent<Renderer>().material = blue;
        else if(c == 4)
            this.transform.gameObject.GetComponent<Renderer>().material = purple;
    }

    void Move()
    { 
        float x = Input.GetAxis("Horizontal") * movespeed;
        float z = Input.GetAxis("Vertical") * movespeed;

        rig.velocity = new Vector3(x, rig.velocity.y, z);
    }

    void TryJump()
    {
        Ray ray = new Ray(this.transform.position, Vector3.down);

        if(Physics.Raycast(ray, 0.7f))
            rig.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
    }

    // sets the player's hat active or not
    public void SetHat(bool hasHat)
    {
        hatObject.SetActive(hasHat);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!photonView.IsMine)
            return;
        // did we hit another player?
        if (collision.gameObject.CompareTag("Player"))
        { 
            // do they have the hat?
            if(GameManager.instance.GetPlayer(collision.gameObject).id == GameManager.instance.playerWithHat) 
            {
                //can we get the hat?
                if (GameManager.instance.CanGetHat())
                {
                    // give us the hat
                    GameManager.instance.photonView.RPC("GiveHat", RpcTarget.All, id, false);
                }
            }
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(curHatTime);
        }
        else if (stream.IsReading)
        {
            curHatTime = (float)stream.ReceiveNext();
        }
    }
}
