using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand_UI : MonoBehaviour {
    public GameObject laser_Obj;
    public Material normal_Mat;
    public Material click_Mat;
    LineRenderer laser;
    
    // Use this for initialization
    void Start () {
        laser = Instantiate(laser_Obj.gameObject.GetComponent<LineRenderer>());
        laser.transform.parent = transform.parent;
        laser.widthMultiplier = 0.2f;
    }
	
	// Update is called once per frame
	void Update () {
        //CheckRayCasting();
        if (Input.GetMouseButtonDown(0))
        {
            print("Ok");
        }
        if (Input.GetMouseButtonUp(1))
        {
            print("End");
        }

    }

    void CheckRayCasting()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1000f))
        {
            if (hit.transform.tag == "Button")
            {

            }
        }
    }

}
