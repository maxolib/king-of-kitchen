using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand_Test : MonoBehaviour {
    public GameObject hand_Obj;
    public GameObject camera_Obj;
    public Rigidbody rb_Camera;            // Rigidbody of Player

    public bool hold;
    public bool movable;
    public Food food;
    public GameObject hold_Obj;

    float maxRange;
    float movable_limit;

    void Start () {
        maxRange = 100f;
        movable_limit = 10f;
        hold = false;
        movable = false;

        rb_Camera = camera_Obj.transform.GetComponent<Rigidbody>();
	}
	
	void Update () {
        if (hold)
        {
            Holding();
        }
        else
        {
            Pointing();
        }
    }

    void Holding()
    {
        if (Input.GetMouseButtonUp(0))
        {
            hold = false;
        }
    }

    void Pointing()
    {
        Pointing();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxRange))
        {
            if (hit.transform != null)
            {
                Transform hit_t = hit.transform.GetComponent<Transform>();

                if (hit_t.tag == "Food")
                {
                    food = hit_t.gameObject.GetComponent<Food>();

                    // click the button
                    if (Input.GetMouseButtonDown(0))
                    {
                        SetObjectHold();
                    }
                }

                if (hit_t.tag == "Movable")
                {
                    if (hit.distance <= movable_limit)
                    {
                        movable = true;
                        // click the button
                        if (Input.GetMouseButtonDown(0))
                        {
                            // add force to move
                            rb_Camera.AddForce(camera_Obj.transform.forward * (400f + hit.distance * 50));
                            movable = true;
                        }
                        else
                        {
                            movable = false;
                        }
                    }
                }
            }
        }

    }

    void SetObjectHold()
    {
        food.held = true;
    }

    void ResetObjectHold()
    {
        food.held = false;
    }



}
