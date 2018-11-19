using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {
    
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("a"))
        {
            transform.Rotate(Time.deltaTime * 100, 0, 0);
        }
        if (Input.GetKeyDown("d"))
        {
            transform.Rotate(-Time.deltaTime * 100, 0, 0);
        }
        if (Input.GetKeyDown("a"))
        {
            transform.Rotate(0, 0, Time.deltaTime * 100);
        }
        if (Input.GetKeyDown("d"))
        {
            transform.Rotate(0, 0, -Time.deltaTime * 100);
        }
    }
}
