using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public Text score_Text;

    float maxRange = 10f;


    void Update () {
        Pointing();
	}

    void Pointing()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxRange))
        {
            if (hit.transform.tag == "Food")
            {
                print("ddd");
                Food obj = hit.transform.gameObject.GetComponent<Food>();
                AddScore(obj.score);
                score_Text.text = obj.score.ToString();
            }

            if (hit.transform.tag == "Movable")
            {

            }
        }
    }

    void AddScore(int score)
    {
        score_Text.text = (int.Parse(score_Text.text) + score) + "";
    }
}
