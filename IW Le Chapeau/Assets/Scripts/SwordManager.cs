using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SwordManager : MonoBehaviour
{
    public GameObject hitBox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            hitBox.SetActive(true);
            Invoke("swingSword", .1f);
        }
    }

    public void swingSword()
    {
        hitBox.SetActive(false);
    }
}
