using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public short Life;
    private BoxCollider2D interactionCollider;

    public void OnInteractionEnter(Collider2D collider)
    {
        if (collider.transform.parent == null || collider.transform.parent.name != "walls")
            InteractingWith = collider.gameObject;
    }

    public void OnInteractionExit(Collider2D collider)
    {
        if (collider.transform.parent == null || collider.transform.parent.name != "walls")
            InteractingWith = null;
    }

    public void OnHit()
    {
        tossBackwards();
        godState();

        Life--;
        if (Life == 0)
            die();
    }

    protected override void Start()
    {
        base.Start();
        interactionCollider = gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>();
        //startDrag = Rb2d.drag;
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

    protected override void UpdateAnimation()
    {
        base.UpdateAnimation();
        rotateInteractionCollider();
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

        Walk(moveDirection);
    }

    private void rotateInteractionCollider()
    {
        if (interactionCollider != null && HeadingVector.sqrMagnitude != 0)
        {
            int angle = HeadingVector.x != 0F ? (int)HeadingVector.x * 90 : (HeadingVector.y > 0F ? -180 : 0);
            interactionCollider.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void tossBackwards()
    {
        Vector3 tossVector = HeadingVector * -1;
        StartCoroutine(MoveCR(tossVector, StepLength * 1.5F, Speed * 3, null));
    }

    private void godState()
    {
        
    }

    private void die()
    {
        throw new NotImplementedException();
    }
}
