using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {
    //Setup Food
    public int score;
    public bool held;
    public bool selected;
    public GameObject particle;
    public GameObject hand;
    
    void Start()
    {
        held = false;
        selected = false;
    }
    void Update()
    {
        if (!selected)
        {
            transform.Rotate(0, 1, 0);
        }
        if (!held && hand != null) {
            Destroy(hand);
        }
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    public void DestroyParticle() {
        Destroy(particle.gameObject);
    }

    public void DestroyGameObjectDelay(int delay)
    {
        //Destroy(gameObject, delay);
        print("Destroy already!!!");
    }

}
