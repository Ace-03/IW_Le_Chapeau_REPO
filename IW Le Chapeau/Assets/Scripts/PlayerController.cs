using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetKeyUp(KeyCode.Space))
            TryJump();
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
    }

    void Move()
    { 
        float x = Input.GetAxis("Horizontal") * movespeed;
        float z = Input.GetAxis("Vertical") * movespeed;

        rig.velocity = new Vector3(x, rig.velocity.y, z);
    }

    void TryJump()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

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
}
