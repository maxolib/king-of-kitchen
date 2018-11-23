using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Hand_Test : MonoBehaviour {
    

    public GameObject SetupScene;
    GameInfo gameInfo;

    public GameObject hand_Obj;
    public GameObject camera_Obj;
    public GameObject hit_Obj;
    public Rigidbody rb_Camera;

    public bool hold;
    public bool movable;
    public Food food;
    public Transform hold_T;
    public int handType;

    float maxRange;
    float movable_limit;

    void Start () {
        gameInfo = SetupScene.transform.GetComponent<GameInfo>();
        maxRange = 100f;
        hold = false;
        movable = false;

        rb_Camera = camera_Obj.transform.GetComponent<Rigidbody>();
	}
	
	void Update () {
        if (hold && hold_T != null)
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
        if (gameInfo.GetGrabUp(handType))
        {
            ResetObjectHold();
            gameInfo.UpdateHold(hold, handType);
        }
    }

    void Pointing()
    {
        hold = false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxRange))
        {

            if (hit.transform != null)
            {
                gameInfo.UpdateDistance(hit.distance, handType);
                if (hit.transform.tag == "Food")
                {
                    // click the button
                    if (gameInfo.GetGrabDown(handType))
                    {
                        print(gameInfo.handType[handType]);
                        hold_T = hit.transform.GetComponent<Transform>();
                        SetObjectHold();
                        gameInfo.UpdateHold(hold, handType);
                    }
                }

                if (hit.transform.tag == "Movable")
                {
                    if (hit.distance <= gameInfo.movable_Limit)
                    {
                        movable = true;
                        // click the button
                        if (gameInfo.GetGrabDown(handType))
                        {
                            // add force to move
                            rb_Camera.AddForce(transform.forward * (400f + hit.distance * 50));
                            movable = true;
                        }
                        else
                        {
                            movable = false;
                        }
                    }
                    gameInfo.UpdateMovable(movable, handType);
                }
            }
        }

    }

    void SetObjectHold()
    {
        food = hold_T.transform.GetComponent<Food>();
        food.held = true;
        food.selected = true;
        hold = true;
        hold_T.position = transform.position;
        hold_T.parent = transform;
        hold_T.localRotation = Quaternion.identity;
        hold_T.GetComponent<Rigidbody>().isKinematic = true;
        
    }

    void ResetObjectHold()
    {
        food.held = false;
        hold_T.GetComponent<Rigidbody>().isKinematic = false;
        hold_T.GetComponent<Rigidbody>().useGravity = true;
        hold_T.parent = null;
        food = null;
        hold_T = null;
        hold = false;

    }



}
