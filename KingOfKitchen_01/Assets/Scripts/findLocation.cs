using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class findLocation : MonoBehaviour {
    public GameObject locationSet;
    public GameObject foodSet;
	void Start () {
        
        Transform[] locationList = locationSet.GetComponentsInChildren<Transform>();
        Transform[] foodList = foodSet.GetComponentsInChildren<Transform>();
        int locationNum = locationList.Length;
        int foodNum = foodList.Length;
        Random r = new Random();
        
        //show all transform
        foreach (Transform t in locationList)
        {
            //int randomNum = Random.Range(0, foodNum);
            //Instantiate(t, foodList[randomNum]);
            print("" + locationNum);
        }




	}
	
}
