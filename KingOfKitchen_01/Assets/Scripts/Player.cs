using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public GameObject SetupScene;
    GameInfo gameInfo;

    LineRenderer laser;
    public GameObject camera_Obj;   // the camera object
    public GameObject hand_Obj;     // the object for testing
    public GameObject hit_Obj;
    public Transform hold_Obj;      // keep the holded object 
    public Text score_Text;         // score text in UI
    public Text distance_Text;      // distance text in UI
    public Text movable_Text;       // movable text in UI
    public Text hold_Text;          // hold text in UI
    public Rigidbody rb;            // Rigidbody of Player
    public string[] mode_Name = {"Ray-Casting", "Homer"};
    public int mode;                // ## The game has 2 modes to switch 2 Techniques ##
                                    // mode 0: Ray-Casting
                                    // mode 1: Homer

    int score;                      // current score of this player
    int count;                      // number of current frame
    float movable_limit;            // limit distance for moving 
    float collect_limit;            // limit distance for collecting food
    bool movable;
    bool hold;
    Color green_Color;
    Color red_Color;

    float maxRange;

    void Start()
    {
        // initial variable
        gameInfo = SetupScene.transform.GetComponent<GameInfo>();
        score = 0;
        count = 0;
        movable_limit = 10f;
        maxRange = 100f;
        movable = false;
        hold = false;
        green_Color = new Color(0, 255, 0);
        red_Color = new Color(255, 0, 0);
        rb = transform.GetComponent<Rigidbody>();
        mode = 0;
        //Cursor.visible = false;
        hit_Obj = (GameObject) Instantiate(gameInfo.hit_Obj);
        hit_Obj.transform.parent = hand_Obj.transform;
        hit_Obj.transform.position = hand_Obj.transform.position;
        hit_Obj.transform.localRotation = Quaternion.identity;


        laser = gameObject.AddComponent<LineRenderer>();
        laser.widthMultiplier = 0.2f;

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
        count++;
    }

    void Pointing()
    {
        RaycastHit hit;
        UpdateMovable(movable);
        if (Physics.Raycast(camera_Obj.transform.position, camera_Obj.transform.forward, out hit, maxRange))
        {
            UpdateHitObject(hit.point, hit.normal);
            if (hit.transform != null)
            {
                // hit.transform is the GameObject that RayCasting hit
                Transform hit_t = hit.transform.GetComponent<Transform>();

                UpdateDistance(hit.distance.ToString());

                if (hit_t.tag == "Food")
                {
                    Food obj = hit_t.gameObject.GetComponent<Food>();

                    if (Input.GetMouseButtonDown(0))
                    {
                        hold_Obj = hit_t;
                        SetHoldObject(hit_t);
                        hold = true;
                        obj.held = true;
                    }
                    //print(count + ": " + hold);
                    UpdateHold(hold);
                    UpdateScore();
                }

                else if (hit_t.tag == "Movable")
                {
                    if (hit.distance <= movable_limit)
                    {
                        movable = true;
                        if (Input.GetMouseButtonDown(0))
                        {
                            rb.AddForce(camera_Obj.transform.forward * (400f + hit.distance * 50));
                            rb.useGravity = true;
                        }

                    }
                    else
                    {
                        movable = false;
                    }
                    UpdateMovable(movable);
                }
            }
        }
        else
        {
            UpdateDistance("-");
        }
    }

    void Holding()
    {
        if (Input.GetMouseButtonUp(0))
        {
            hold = false;
            ResetHoldObject();
        }
        UpdateHold(hold);
    }

    void SetHoldObject(Transform obj)
    {
        obj.transform.position = hand_Obj.transform.position;
        //obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.GetComponent<Rigidbody>().isKinematic = true;
    }

    void ResetHoldObject()
    {
        //hold_Obj.parent = null;
        hold_Obj.GetComponent<Rigidbody>().isKinematic = false;
        hold_Obj.GetComponent<Rigidbody>().useGravity = true;

        hold_Obj = null;

    }

    void AddScore(int scoreFood)
    {
        score += scoreFood;
    }

    void UpdateScore()
    {
        score_Text.text = score.ToString();
    }

    void UpdateDistance(string text)
    {
        if (text == null)
        {
            distance_Text.text = "-";
        }
        else if (text is string)
        {
            distance_Text.text = text;
        }
    }

    void UpdateMovable(bool movable)
    {
        if (movable)
        {
            movable_Text.text = "True";
            movable_Text.color = green_Color;
        }
        else
        {
            movable_Text.text = "False";
            movable_Text.color = red_Color;
        }
    }

    void UpdateHold(bool hold)
    {
        if (hold)
        {
            hold_Text.text = "True";
            hold_Text.color = green_Color;
        }
        else
        {
            hold_Text.text = "False";
            hold_Text.color = red_Color;
        }
    }
    
    void UpdateHitObject(Vector3 position, Vector3 normal)
    {
        laser.SetPosition(0, hand_Obj.transform.position);
        laser.SetPosition(1, position);
        hit_Obj.transform.position = position;
        hit_Obj.transform.localRotation = Quaternion.LookRotation(normal);
    }
}
