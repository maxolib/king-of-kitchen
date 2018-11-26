using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class Hand_Test : MonoBehaviour {
    // Import GameInfo status
    public GameObject SetupScene;
    GameInfo gameInfo;

    // Prefab of any GameObject
    public GameObject hand_Obj;
    public GameObject head_Obj;
    public GameObject camera_Obj;
    public GameObject hit_Obj;
    public Rigidbody rb_Camera;
    public Rigidbody simulator;
    LineRenderer laser;

    // Common valiable
    public int handType;
    public bool hold;
    public bool movable;
    public bool click;
    public bool grab_Hold;
    public float hold_Distance;
    public float click_Distance;
    public Food food;
    public Vector3 click_Position;
    public Vector3 click_Direction;
    public Transform hold_T;

    // Local value

    void Start () {
        // Common value of Hand
        gameInfo = SetupScene.transform.GetComponent<GameInfo>();
        click = false;
        hold = false;
        movable = false;
        grab_Hold = false;
        hit_Obj = (GameObject)Instantiate(gameInfo.hit_Obj);
        click_Direction = transform.forward;

        // Rigibody Simulation and VR-Camera Setup
        rb_Camera = head_Obj.transform.GetComponent<Rigidbody>();
        simulator = new GameObject().AddComponent<Rigidbody>();
        simulator.name = "simulator";
        simulator.transform.parent = transform.parent;

        // LineRenderer setup
        laser = Instantiate(gameInfo.Laser_Obj.gameObject.GetComponent<LineRenderer>());
        laser.transform.parent = transform.parent;
        laser.widthMultiplier = 0.2f;
    }
	
	void Update () {
        // this Hand is holding the object 
        if (hold && hold_T != null)
        {
            Holding();
            UpdateHitObject(transform.position, Vector3.zero);
        }
        // Enable Ray-Casting point out from Hand
        else
        {
            Pointing();
        }
    }

    void Holding()
    {
        simulator.velocity = (transform.position - simulator.transform.position)  * 100f;
        // Homer technique
        if (gameInfo.modeID == 1)
        {
            float treshold = gameInfo.hand_Limit / 2;
            float distance = gameInfo.FindDistanceIgnoreY(transform.position, camera_Obj.transform.position);
            if (distance >= treshold)
            {
                hold_T.position += transform.forward * 0.1f;
            }
            else {
                hold_T.position -= transform.forward * 0.1f;
            }
        }


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
        // Create Ray-casting
        if (Physics.Raycast(transform.position, transform.forward, out hit, gameInfo.maxRange))
        {
            if (hit.transform != null)
            {
                gameInfo.UpdateDistance(hit.distance, handType);
                temp[0] = hit.point;
                temp[1] = hit.normal;
                if (hit.transform.tag == "Food" && hit.distance <= gameInfo.collect_Limit)
                {
                    if (gameInfo.GetGrabDown(handType))
                    {
                        hold_T = hit.transform.GetComponent<Transform>();
                        hold_Distance = hit.distance;
                        SetObjectHold();
                        gameInfo.UpdateHold(hold, handType);
                    }
                }
                else if (hit.transform.tag == "Movable" && hit.distance <= gameInfo.movable_Limit)
                {
                    movable = true;
                    // Check holding of Coltroller
                    if (gameInfo.GetGrabDown(handType))
                    {
                        grab_Hold = true;
                    }
                    else if (gameInfo.GetGrabUp(handType))
                    {
                        grab_Hold = false;
                    }
                    if (grab_Hold)
                    {
                        // Keep click point information
                        if (!click)
                        {
                            click_Position = transform.position;
                            click_Direction = transform.forward;
                            click_Distance = hit.distance;
                            click = true;
                        }

                        // Add force every frame
                        float d = gameInfo.FindDistanceIgnoreY(click_Position, transform.position);
                        float M = gameInfo.hand_Limit;
                        float p = ((d / M ) * click_Distance) + 20f;
                        Vector3 power = GetVectorAddForce(p);
                        rb_Camera.AddForce(power);
                        movable = true;
                    }
                    else
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
                // Add force when hit the jumable object
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
        // Update position of the hit object in each case
        UpdateHitObject(temp[0], temp[1]);
    }

    void SetObjectHold()
    {
        food = hold_T.transform.GetComponent<Food>();
        food.held = true;
        food.selected = true;
        hold = true;
        hold_T.parent = transform;
        hold_T.localRotation = Quaternion.identity;
        hold_T.GetComponent<Rigidbody>().useGravity = false;
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

    Vector3 GetVectorAddForce(float power)
    {
        Vector3 result = new Vector3(transform.forward.x * power, 0, transform.forward.z * power);
        float power_Limit = 100f;
        if (result.x >= power_Limit)
        {
            result.x = power_Limit;
        }
        if (result.z >= power_Limit)
        {
            result.z = power_Limit;
        }
        return result;
    }

}
