using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.SceneManagement;

public class Hand_UI : MonoBehaviour {
    public GameObject laser_Obj;
    public Material normal_Mat;
    public Material click_Mat;

    Color normal_Color;
    Color click_Color;
    LineRenderer laser;
    
    // Use this for initialization
    void Start () {
        click_Color = new Color(0, 255, 0);
        normal_Color = new Color(0, 255, 0);


        laser = Instantiate(laser_Obj.gameObject.GetComponent<LineRenderer>());
        laser.transform.parent = transform.parent;
        laser.widthMultiplier = 0.2f;
    }
	
	// Update is called once per frame
	void Update () {
        CheckRayCasting();
    }

    void CheckRayCasting()
    {
        RaycastHit hit;
        Vector3 temp = transform.position;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1000f))
        {
            temp = hit.point;
            if (hit.transform.tag == "Button")
            {
                Material m_Material;
                string buttonType = hit.transform.GetComponent<Button_UI>().name;
                m_Material = hit.transform.GetComponent<Renderer>().material;

                if (GetGrabDown(0))
                {
                    print("clicked");
                    m_Material.color = click_Color;
                }
                if (GetGrabUp(0))
                {
                    print("up");
                    m_Material.color = normal_Color;
                    print(buttonType);
                    if (buttonType == "start" || buttonType == "restart")
                    {
                        SceneManager.LoadScene("Main_Scene");
                    }
                    else if (buttonType == "quit")
                    {
                        Application.Quit();
                    }
                }
            }
        }
        UpdateLinerenderer(temp);
    }
    void UpdateLinerenderer(Vector3 position)
    {
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, position);
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

}
