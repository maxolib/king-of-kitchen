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
    public Rigidbody simulator;

    public int handType;
    public bool hold;
    public bool movable;
    public Food food;
    public Transform hold_T;

    public LineRenderer laser;

    float maxRange;
    float movable_limit;

    void Start () {
        gameInfo = SetupScene.transform.GetComponent<GameInfo>();
        maxRange = 100f;
        hold = false;
        movable = false;
        hit_Obj = (GameObject)Instantiate(gameInfo.hit_Obj);

        rb_Camera = camera_Obj.transform.GetComponent<Rigidbody>();
        simulator = new GameObject().AddComponent<Rigidbody>();
        simulator.name = "simulator";
        simulator.transform.parent = transform.parent;

        // LineRenderer
        laser = gameObject.AddComponent<LineRenderer>();
        laser.widthMultiplier = 0.02f;
	}
	
	void Update () {
        if (hold && hold_T != null)
        {
            Holding();
            UpdateHitObject(transform.position, Vector3.zero);
        }
        else
        {
            Pointing();
        }
    }

    void Holding()
    {
        simulator.velocity = (transform.position - simulator.transform.position) * 50f;
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
        Vector3[] temp = {transform.position, Vector3.zero};
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxRange))
        {
            if (hit.transform != null)
            {
                gameInfo.UpdateDistance(hit.distance, handType);
                temp[0] = hit.point;
                temp[1] = hit.normal;
                if (hit.transform.tag == "Food" && hit.distance <= gameInfo.collect_Limit)
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
                else if (hit.transform.tag == "Movable" && hit.distance <= gameInfo.movable_Limit)
                {
                    movable = true;
                    // click the button
                    if (gameInfo.GetGrabDown(handType))
                    {
                        // add force to move
                        rb_Camera.AddForce(transform.forward.x * (400f + hit.distance * 50), 0, transform.forward.z * (400f + hit.distance * 50));
                        movable = true;
                    }
                    if (gameInfo.GetTeleportDown(handType))
                    {
                        rb_Camera.AddForce(0, 400f, 0);
                    }
                    else
                    {
                        movable = false;
                    }
                    gameInfo.UpdateMovable(movable, handType);
                }
                else
                {
                    temp[0] = transform.position;
                    temp[1] = Vector3.zero;
                }
            }
        }
        UpdateHitObject(temp[0], temp[1]);
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
        hold_T.GetComponent<Rigidbody>().velocity = simulator.velocity;
        hold_T.parent = null;
        food = null;
        hold_T = null;
        hold = false;

    }

    void UpdateHitObject(Vector3 position, Vector3 normal)
    {
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, position);
        hit_Obj.transform.position = position;
        hit_Obj.transform.localRotation = Quaternion.LookRotation(normal);
    }

}
