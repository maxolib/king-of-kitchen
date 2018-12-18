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
    public GameObject hand;
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
    public Vector3 hold_point;
    public Vector3 hand_position;
    public Vector3 hand_rotation;
    public Transform hold_T;
    public float scal;

    // Local value

    void Start () {
        // Common value of Hand
        gameInfo = SetupScene.transform.GetComponent<GameInfo>();
        click = false;
        hold = false;
        movable = false;
        grab_Hold = false;
        scal = 0f;
        hit_Obj = (GameObject)Instantiate(gameInfo.hit_Obj);
        click_Direction = transform.forward;

        // Rigibody Simulation and VR-Camera Setup
        rb_Camera = head_Obj.transform.GetComponent<Rigidbody>();
        rb_Camera.maxAngularVelocity = gameInfo.velocity_Limit;
        simulator = new GameObject().AddComponent<Rigidbody>();
        simulator.name = "simulator";
        simulator.transform.parent = transform.parent;

        // LineRenderer setup
        laser = Instantiate(gameInfo.Laser_Obj.gameObject.GetComponent<LineRenderer>());
        laser.transform.parent = transform.parent;
        laser.widthMultiplier = 0.2f;

        // Create hand object
        //hand = Instantiate(gameInfo.hand_Obj);
        //hand.transform.position = transform.position;
        //hand.transform.rotation = transform.rotation;
        //hand.transform.parent = transform.parent;
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
        
        if (gameInfo.modeID == 0) {
            Vector3 p = (transform.position - hand_position) * 3.5f;
            Vector3 r = GetRotation(transform) - hand_rotation;
            hold_T.position += p;
            hold_T.localRotation = transform.rotation;
        }
        else {
            float distance_real = gameInfo.FindDistanceIgnoreY(transform.position, head_Obj.transform.position);
            Vector3 p = (transform.position - hand_position) * (3.5f + scal*distance_real);
            Vector3 r = GetRotation(transform) - hand_rotation;
            hold_T.position += p;
            hold_T.localRotation = transform.rotation;
        }
        hand_position = transform.position;
        hand_rotation = GetRotation(transform);

        // Homer technique
        /*
        if (gameInfo.modeID == 1)
        {
            float treshold = gameInfo.hand_Limit / 2;
            float distance = gameInfo.FindDistance(transform.position, camera_Obj.transform.position);
            if (distance >= treshold)
            {
                hold_T.position += transform.forward * 0.1f;
            }
            else {
                hold_T.position -= transform.forward * 0.1f;
            }
        }
        */


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
        Vector3[] temp = {GetMaxDistance(), GetRotation(transform)};
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
                        gameInfo.select_Sound.Play();
                        hold_T = hit.transform.GetComponent<Transform>();
                        hold_Distance = hit.distance;
                        hold_point = hit.point;
                        SetObjectHold();
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
                    temp[0] = GetMaxDistance();
                    temp[1] = GetRotation(transform);
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

        // Set hand object to Food
        food.hand = Instantiate(hand, hold_point, transform.rotation);
        food.hand.transform.parent = food.transform;

        // Set hand not active
        hand.gameObject.SetActive(false);
        hit_Obj.gameObject.SetActive(false);

        hand_position = transform.position;
        hand_rotation = GetRotation(transform);

        float distance_object = gameInfo.FindDistanceIgnoreY(hold_T.position, head_Obj.transform.position);
        float distance_real = gameInfo.FindDistanceIgnoreY(transform.position, head_Obj.transform.position);
        scal = distance_object / distance_real;
        //hold_T.parent = transform;
        //hold_T.localRotation = Quaternion.identity;
        hold_T.GetComponent<Rigidbody>().useGravity = false;
        hold_T.GetComponent<Rigidbody>().isKinematic = true;

        //Set particle
        food.particle = Instantiate(gameInfo.particle_food);
        food.particle.transform.position = food.transform.position;
        food.particle.transform.parent = food.transform;


        gameInfo.UpdateHold(hold, handType);
    }

    void ResetObjectHold()
    {
        food.DestroyParticle();
        food.held = false;
        hold_T.GetComponent<Rigidbody>().isKinematic = false;
        hold_T.GetComponent<Rigidbody>().useGravity = true;
        hold_T.GetComponent<Rigidbody>().velocity = simulator.velocity;
        hold_T.parent = null;
        food = null;
        hold_T = null;
        hold = false;

        hand.gameObject.SetActive(true);
        hit_Obj.gameObject.SetActive(true);

    }

    void UpdateHitObject(Vector3 position, Vector3 normal)
    {
        if (position == new Vector3(-1000, -1000, -1000)) {
            laser.SetPosition(0, position);
        } else {
            laser.SetPosition(0, transform.position);
        }
        laser.SetPosition(1, position);
        hit_Obj.transform.position = position;
        hit_Obj.transform.localRotation = Quaternion.LookRotation(normal);
    }

    void UpdateHandObject(Vector3 position, Vector3 normal) {
        hand.transform.position = position;
        hand.transform.localRotation = Quaternion.LookRotation(normal);
    }

    Vector3 GetVectorAddForce(float power)
    {
        Vector3 result = new Vector3(transform.forward.x * power, 0, transform.forward.z * power);
        if (result.x >= gameInfo.force_Limit)
        {
            result.x = gameInfo.force_Limit;
        }
        if (result.z >= gameInfo.force_Limit)
        {
            result.z = gameInfo.force_Limit;
        }
        return result;
    }

    Vector3 GetMaxDistance() {
        return transform.forward * 100000f;
    }

    Vector3 GetRotation(Transform t) {
        return new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
    }

    

}
