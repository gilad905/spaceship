using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float Speed;
    public enum Direction { None, Up, Right, Down, Left };

    protected GameObject InteractingWith = null;
    protected Rigidbody2D Rb2d;
    protected const float StepLength = 0.24F;
    protected delegate void moveEndedHandler();
    protected bool Standing;
    protected Direction Heading
    {
        get
        {
            return _heading;
        }
        set
        {
            _heading = value;
            HeadingVector = GetDirectionVector(value);
        }
    }
    protected Vector3 HeadingVector;

    float startScaleX;
    Animator animator;
    Direction _heading;
    bool inCollision = false;

    protected virtual void Start()
    {
        Rb2d = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        startScaleX = transform.localScale.x;
        Standing = true;
        Heading = Direction.Down;
    }

    protected virtual void Update()
    {
    }

    protected virtual void FixedUpdate()
    {
    }

    protected void Walk(Direction direction)
    {
        bool standingChanged = (Standing != (direction == Direction.None));
        bool headingChanged = (direction != Direction.None && direction != Heading);

        if (standingChanged || headingChanged)
        {
            if (standingChanged)
                Standing = !Standing;
            if (headingChanged)
                Heading = direction;

            Vector3 dirVector = GetDirectionVector(direction);
            Rb2d.velocity = dirVector * Speed;
            UpdateAnimation();
        }
    }

    protected IEnumerator MoveCR(Vector3 dirVector, float distance, float speed, moveEndedHandler moveEndedHandler)
    { 
        float sqrRemainingDistance;

        if (dirVector.sqrMagnitude == 0)
            yield break;

        Vector3 endPosition = transform.position + (dirVector * distance);

        do
        {
            Vector2 updatePosition = Vector3.MoveTowards(Rb2d.position, endPosition, speed * Time.deltaTime);
            Rb2d.MovePosition(updatePosition);
            sqrRemainingDistance = (transform.position - endPosition).sqrMagnitude;

            yield return null;
        }
        while (!inCollision && sqrRemainingDistance > float.Epsilon);

        if (moveEndedHandler != null)
            moveEndedHandler();
    }

    protected virtual void UpdateAnimation()
    {
        animator.SetInteger("moveX", (Standing ? 0 : (int)HeadingVector.x));
        animator.SetInteger("moveY", (Standing ? 0 : (int)HeadingVector.y));
        animator.SetBool("standing", Standing);

        if (HeadingVector.x != 0)
            transform.localScale = new Vector3(startScaleX * HeadingVector.x, transform.localScale.y, transform.localScale.z);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        inCollision = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        inCollision = false;
    }
}
