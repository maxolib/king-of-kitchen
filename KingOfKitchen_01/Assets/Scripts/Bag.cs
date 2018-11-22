using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour {
    public GameObject SetupScene;
    GameInfo gameInfo;

	void Start () {
        gameInfo = SetupScene.transform.GetComponent<GameInfo>();
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Food")
        {
            Food food = col.transform.GetComponent<Food>();
            if (food.selected)
            {
                gameInfo.AddScore(food.score);
                food.DestroyGameObject();
            }
        }
    }
}
