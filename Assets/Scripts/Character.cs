using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float Speed;
    public enum Direction {None, Up, Right, Down, Left };

    protected BoxCollider2D InteractionCollider = null;
    protected GameObject Interactor = null;
    protected Rigidbody2D Rb2d = null;

    float startScaleX;
    Animator animator = null;
    Direction lastMovement = Direction.None;
    const float stepDistance = 0.241F;

    protected virtual void Start()
    {
        Rb2d = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        InteractionCollider = gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>();
        startScaleX = transform.localScale.x;
    }

    protected virtual void Update()
    {
    }

    protected virtual void FixedUpdate()
    {
    }

    protected virtual void OnInteractionEnter(Collider2D collider)
    {
    }

    protected virtual void OnInteractionExit(Collider2D collider)
    {
    }

    protected virtual void MoveOnInput()
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

    protected void Move(Direction direction)
    {
        if (direction != lastMovement)
        {
            lastMovement = direction;
            Vector3 dirVector = GetDirectionVector(direction);
            Vector2 movement = new Vector2(dirVector.x, dirVector.y);
            Rb2d.velocity = movement * Speed;
            MovementActions(dirVector);
        }
    }

    protected void MovementActions(Direction direction)
    {
        Vector3 dirVector = GetDirectionVector(direction);
        MovementActions(dirVector);
    }

    protected void MovementActions(Vector3 dirVector)
    {
        animate(dirVector);
        rotateInteractionCollider(dirVector);
    }

    protected Vector3 GetDirectionVector(Direction direction)
    {
        #pragma warning disable IDE0017 // Simplify object initialization
        Vector3 result = new Vector3();
        #pragma warning restore IDE0017 // Simplify object initialization

        result.x = (direction == Direction.Right ? 1 : (direction == Direction.Left ? -1 : 0));
        if (result.x == 0)
            result.y = (direction == Direction.Up ? 1 : (direction == Direction.Down ? -1 : 0));

        return result;
    }

    private void animate(Vector3 dirVector)
    {
        animator.SetInteger("moveX", (int)dirVector.x);
        animator.SetInteger("moveY", (int)dirVector.y);
        animator.SetBool("idle", dirVector.sqrMagnitude == 0);

        if (dirVector.x != 0)
            transform.localScale = new Vector3(startScaleX * dirVector.x, transform.localScale.y, transform.localScale.z);
    }

    private void rotateInteractionCollider(Vector3 dirVector)
    {
        if (InteractionCollider != null && dirVector.sqrMagnitude != 0)
        {
            int angle = dirVector.x != 0F ? (int)dirVector.x * 90 : (dirVector.y > 0F ? -180 : 0);
            InteractionCollider.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
