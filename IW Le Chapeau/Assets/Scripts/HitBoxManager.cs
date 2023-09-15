using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HitBoxManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            Debug.Log("Swung Sword");
            if (player.gameObject.GetComponent<PlayerController>().id != GameManager.instance.playerWithHat)
            {
                Debug.Log("Got Hit");
                player.gameObject.GetComponent<PlayerController>().rig.AddForce((new Ray(this.transform.position, player.transform.position)).direction, ForceMode.Impulse);
            }

        }
    }

      
}
