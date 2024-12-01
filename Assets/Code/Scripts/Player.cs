using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform leftHand, rightHand;
    public static Player instance { get; private set; } // singleton

    public void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(instance); // what if scene change?
        }
        Player.instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
