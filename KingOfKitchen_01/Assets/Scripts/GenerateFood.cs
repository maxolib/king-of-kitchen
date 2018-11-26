using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFood : MonoBehaviour {
    public GameObject locationSet;
    public GameObject foodSet;
    public float ratio;

    // Use this for initialization
    void Start () {
        // get all objects into Arrays
        Transform[] locationList_tmp = locationSet.GetComponentsInChildren<Transform>();
        Transform[] foodList_tmp = foodSet.GetComponentsInChildren<Transform>();
        // convert Array to List
        List<Transform> locationList = new List<Transform>(locationList_tmp);
        List<Transform> foodList = new List<Transform>(foodList_tmp);

        int locationNum = locationList.Count;
        int foodNum = foodList.Count;
        //int setNum = CalculateSetNumber(locationNum, ratio);
        
        
        //show all transform
        foreach (Transform t in locationList)
        {
            if (t == locationList[0])
            {
                continue;
            }

            int randomNum = Random.Range(1, foodNum);
            Instantiate(foodList[randomNum].transform, t.position, Quaternion.identity);
            //var temp = Instantiate(foodList[randomNum].transform, t.position, Quaternion.identity);

            //Destroy(temp, 5);

        }
    }

    int CalculateSetNumber(int total, float ratio)
    {
        int result = (int)(total * ratio);
        return result;
    }

    List<Transform> RandomPosition(List<Transform> list, int count)
    {
        
        return null;
    }
    

}
