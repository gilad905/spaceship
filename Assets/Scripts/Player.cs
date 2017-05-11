using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Interactor != null)
            Interactor.SendMessage("Interact");
    }

    protected override void FixedUpdate()
    {
        base.MoveOnInput();
    }

    protected override void OnInteractionEnter(Collider2D collider)
    {
        if (collider.transform.parent.name != "walls")
            Interactor = collider.gameObject;
    }

    protected override void OnInteractionExit(Collider2D collider)
    {
        if (collider.transform.parent.name != "walls")
            Interactor = null;
    }
}
