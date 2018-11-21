using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInfo : MonoBehaviour {
    public Text score_Text;
    public int currentScore;

    void Start()
    {
        currentScore = 0;
    }

    public void AddScore(int score)
    {
        currentScore += score;
        UpdateScore();
    }

    public void UpdateScore()
    {
        score_Text.text = currentScore.ToString();
    }


}
