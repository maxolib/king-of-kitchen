using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndScript : MonoBehaviour {
    public TextMeshPro score_Text;
    int score_end;
    // Use this for initialization
    void Start () {
        score_end = 0;
		GameObject temp = GameObject.Find("SetupScene");
        if (temp != null)
        {
            score_end = temp.transform.GetComponent<GameInfo>().currentScore;
            score_Text.SetText(score_end.ToString());
        }

        Destroy(temp);
    }
	
}
