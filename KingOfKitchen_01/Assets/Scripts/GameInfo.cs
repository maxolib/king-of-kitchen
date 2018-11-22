using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class GameInfo : MonoBehaviour {
    public Text score_Text;
    public int currentScore;

    public GameObject hit_Obj;

    public string[] handType = {"Any", "Left", "Right"};

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

    public bool GetGrabDown(int handType)
    {
        if (handType == 0)
        {
            return SteamVR_Input._default.inActions.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any);
        }
        else if (handType == 1)
        {
            return SteamVR_Input._default.inActions.GrabPinch.GetStateDown(SteamVR_Input_Sources.LeftHand);
        }
        else if (handType == 2)
        {
            return SteamVR_Input._default.inActions.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand);
        }
        else
        {
            return false;
        }
    }

    public bool GetGrabUp(int handType)
    {
        if (handType == 0)
        {
            return SteamVR_Input._default.inActions.GrabPinch.GetStateUp(SteamVR_Input_Sources.Any);
        }
        else if (handType == 1)
        {
            return SteamVR_Input._default.inActions.GrabPinch.GetStateUp(SteamVR_Input_Sources.LeftHand);
        }
        else if (handType == 2)
        {
            return SteamVR_Input._default.inActions.GrabPinch.GetStateUp(SteamVR_Input_Sources.RightHand);
        }
        else
        {
            return false;
        }
    }

    public float FindDistance(Vector3 s, Vector3 d)
    {
        return Vector3.Distance(s, d);
    }
    


}
