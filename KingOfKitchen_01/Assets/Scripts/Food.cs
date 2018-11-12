using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {
    //Setup Food
    public int score;
    
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    public void DestroyGameObjectDelay(int delay)
    {
        //Destroy(gameObject, delay);
        print("Destroy already!!!");
    }

}
