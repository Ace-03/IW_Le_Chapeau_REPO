using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviour
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

}
