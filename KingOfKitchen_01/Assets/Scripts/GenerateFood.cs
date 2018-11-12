using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFood : MonoBehaviour {
    public GameObject locationSet;
    public GameObject foodSet;

    // Use this for initialization
    void Start () {
        Transform[] locationList = locationSet.GetComponentsInChildren<Transform>();
        Transform[] foodList = foodSet.GetComponentsInChildren<Transform>();
        int locationNum = locationList.Length;
        int foodNum = foodList.Length;

        print("" + foodNum);
        //show all transform
        foreach (Transform t in locationList)
        {
            if (t == locationList[0])
            {
                continue;
            }

            int randomNum = Random.Range(1, foodNum);
            var temp = Instantiate(foodList[randomNum].transform, t.position, Quaternion.identity);

            Destroy(temp, 5);

        }
    }
	
}
