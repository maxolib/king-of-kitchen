using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Hand_Test : MonoBehaviour {
    
    public GameObject SetupScene;
    GameInfo gameInfo;

    public GameObject hand_Obj;
    public GameObject head_Obj;
    public GameObject camera_Obj;
    public GameObject hit_Obj;
    public Rigidbody rb_Camera;
    public Rigidbody simulator;

    public int handType;
    public bool hold;
    public bool movable;
    public bool click;
    public Vector3 click_Position;
    public Vector3 click_Direction;
    public float click_Distance;
    public Food food;
    public Transform hold_T;

    LineRenderer laser;

    float maxRange;
    float movable_limit;

    void Start () {
        gameInfo = SetupScene.transform.GetComponent<GameInfo>();
        maxRange = 100f;
        hold = false;
        movable = false;
        hit_Obj = (GameObject)Instantiate(gameInfo.hit_Obj);

        rb_Camera = head_Obj.transform.GetComponent<Rigidbody>();
        simulator = new GameObject().AddComponent<Rigidbody>();
        simulator.name = "simulator";
        simulator.transform.parent = transform.parent;

        // LineRenderer
        //laser = gameObject.AddComponent<LineRenderer>();
        laser = Instantiate(gameInfo.Laser_Obj.gameObject.GetComponent<LineRenderer>());
        laser.transform.parent = transform.parent;
        laser.widthMultiplier = 0.2f;
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
        //print(camera_Obj.transform.position.ToString() + ", " + transform.position.ToString() + " ###" + gameInfo.FindDistanceIgnoreY(camera_Obj.transform.position, transform.position));
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
                        if (!click)
                        {
                            click_Position = transform.position;
                            click_Direction = transform.forward;
                            click_Distance = hit.distance;
                            click = true;
                        }

                        float d = gameInfo.FindDistanceIgnoreY(click_Position, transform.position);
                        float M = gameInfo.hand_Limit;
                        float p = ((d / M) * (click_Distance + 200f));
                        //rb_Camera.AddForce(transform.forward.x * p, 0, transform.forward.z * p);
                        print("##############" + p);
                        rb_Camera.AddForce(transform.forward.x * (400f + hit.distance * 50), 0, transform.forward.z * (400f + hit.distance * 50));
                        movable = true;
                    }
                    else if (gameInfo.GetGrabUp(handType))
                    {
                        click = false;
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

                else if (hit.transform.tag == "Jumpable" && hit.distance <= gameInfo.jumpable_Limit)
                {
                    if (gameInfo.GetGrabDown(handType))
                    {

                        rb_Camera.AddForce(0f, ((hit.point.y - camera_Obj.transform.position.y) * 80f) + 200f, 0f);
                    }
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
