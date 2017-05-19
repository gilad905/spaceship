using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private BoxCollider2D interactionCollider;
    bool inGodState = false;

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

    public override void OnHit()
    {
        if (!inGodState)
        {
            base.OnHit();
            startGodState();
            tossBackwards();
        }
    }

    protected override void Start()
    {
        base.Start();
        interactionCollider = gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>();
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (InteractingWith != null)
                InteractingWith.SendMessage("Interact");
            else
                Inventory.UseCurrentItem();
        }
    }

    protected override void FixedUpdate()
    {
        if (!IsMoved)
            walkOnInput();
    }

    protected override void Die()
    {
        base.Die();
        Debug.Log("Game Over, Man!");
    }

    protected override void UpdateAnimation()
    {
        base.UpdateAnimation();
        rotateInteractionCollider();
    }

    private void startGodState()
    {
        inGodState = true;
        Animator.SetBool("godState", true);
        StartCoroutine(Timer(1, godStateEnded));
    }

    private void godStateEnded()
    {
        inGodState = false;
        Animator.SetBool("godState", false);
    }

    private void walkOnInput()
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
        MoveTime(tossVector, 0.2F, Speed * 3);
    }
}
