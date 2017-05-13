using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCollider : MonoBehaviour {
    Player parentCtrl = null;

    private void Start()
    {
        parentCtrl = gameObject.GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (parentCtrl != null)
            parentCtrl.OnInteractionEnter(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (parentCtrl != null)
            parentCtrl.OnInteractionExit(other);
    }
}
