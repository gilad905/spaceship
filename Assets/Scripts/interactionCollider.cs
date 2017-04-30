using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCollider : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.transform.parent.SendMessage("OnInteractionEnter", other);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        gameObject.transform.parent.SendMessage("OnInteractionExit", other);
    }
}
