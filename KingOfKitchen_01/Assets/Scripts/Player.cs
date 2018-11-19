using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public GameObject camera_Obj;
    public Text score_Text;
    public Text distance_Text;
    public Text movable_Text;

    int score;
    int count;
    float movable_limit;
    float collect_limit;
    bool movable;
    Color green_Color;
    Color red_Color;

    float maxRange;

    void Start()
    {
        score = 0;
        count = 0;
        movable_limit = 10f;
        maxRange = 100f;
        movable = false;
        green_Color = new Color(0, 255, 0);
        red_Color = new Color(255, 0, 0);
    }

    void Update () {
        Pointing();
        count++;
    }

    void Pointing()
    {
        RaycastHit hit;
        UpdateMovable(movable);
        if (Physics.Raycast(camera_Obj.transform.position, camera_Obj.transform.forward, out hit, maxRange))
        {
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
                        AddScore(obj.score);
                        obj.DestroyGameObject();
                    }
                    UpdateScore();
                }

                else if (hit_t.tag == "Movable")
                {
                    if (hit.distance <= movable_limit)
                    {
                        movable = true;
                        if (Input.GetMouseButtonDown(0))
                        {
                            print("click: " + count);
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

}
