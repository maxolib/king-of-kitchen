using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Test : MonoBehaviour {
    public GameObject senupScene;
    GameInfo gameInfo;

    Rigidbody rb;


    void Start()
    {
        gameInfo = senupScene.transform.GetComponent<GameInfo>();
        rb = transform.GetComponent<Rigidbody>();
    }
	
	void Update()
    {
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, gameInfo.velocity_Limit);
	}
    
}
