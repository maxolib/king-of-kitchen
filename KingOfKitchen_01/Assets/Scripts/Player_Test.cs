using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Test : MonoBehaviour {
    public GameObject senupScene;
    public GameInfo gameInfo;

    Rigidbody rb;


    void Start()
    {
        gameInfo = senupScene.transform.GetComponent<GameInfo>();
        rb = transform.GetComponent<Rigidbody>();
    }
	
	void Update () {
        //CheckVelocity();
	}

    void CheckVelocity()
    {
        Vector3 velocity = rb.velocity;
        if (velocity.x >= gameInfo.velocity_Limit)
        {
            velocity.x = gameInfo.velocity_Limit;
        }
        if (velocity.y >= gameInfo.velocity_Limit)
        {
            velocity.y = gameInfo.velocity_Limit;
        }
        if (velocity.z >= gameInfo.velocity_Limit)
        {
            velocity.z = gameInfo.velocity_Limit;
        }
        rb.velocity = velocity;
    }
}
