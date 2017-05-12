using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private BoxCollider2D interactionCollider = null;

    protected override void Start()
    {
        base.Start();
        interactionCollider = gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>();
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && InteractingWith != null)
            InteractingWith.SendMessage("Interact");
    }

    protected override void FixedUpdate()
    {
        moveOnInput();
    }

    protected override void OnInteractionEnter(Collider2D collider)
    {
        if (collider.transform.parent.name != "walls")
            InteractingWith = collider.gameObject;
    }

    protected override void OnInteractionExit(Collider2D collider)
    {
        if (collider.transform.parent.name != "walls")
            InteractingWith = null;
    }

    private void moveOnInput()
    {
        Direction moveDirection = Direction.None;

        if (Input.GetKey(KeyCode.RightArrow))
            moveDirection = Direction.Right;
        else if (Input.GetKey(KeyCode.LeftArrow))
            moveDirection = Direction.Left;
        else if (Input.GetKey(KeyCode.UpArrow))
            moveDirection = Direction.Up;
        else if (Input.GetKey(KeyCode.DownArrow))
            moveDirection = Direction.Down;

        Move(moveDirection);
    }

    private void rotateInteractionCollider(Vector3 dirVector)
    {
        if (interactionCollider != null && dirVector.sqrMagnitude != 0)
        {
            int angle = dirVector.x != 0F ? (int)dirVector.x * 90 : (dirVector.y > 0F ? -180 : 0);
            interactionCollider.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    protected override void MovementActions(Vector3 dirVector)
    {
        base.MovementActions(dirVector);
        rotateInteractionCollider(dirVector);
    }
}
