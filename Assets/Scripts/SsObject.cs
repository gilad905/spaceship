using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SsObject : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D other)
    {
        Interact();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Interact();
    }

    void Interact()
    {
        Debug.Log("Collide!");
    }

    bool PlayerIsFacing()
    {
        throw new System.NotImplementedException();
    }

    void Start () {
	}
	
	void Update () {
	}
}