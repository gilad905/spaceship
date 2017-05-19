using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float Speed;
    public enum Direction { None, Up, Right, Down, Left };
    public short Life;
    public GameObject Shot;

    protected delegate void simpleFunc();
    protected GameObject InteractingWith = null;
    protected Rigidbody2D Rb2d;
    protected Animator Animator;
    protected BoxCollider2D Collider;
    protected Direction Heading
    {
        get
        {
            return _heading;
        }
        private set
        {
            _heading = value;
            HeadingVector = GetDirectionVector(value);
        }
    }
    protected Vector3 HeadingVector;
    protected const float StepLength = 0.24F;
    protected bool Standing { get; private set; }
    protected bool IsMoved;

    float startScaleX;
    Direction _heading;
    bool inCollision;

    protected virtual void Start()
    {
        Rb2d = gameObject.GetComponent<Rigidbody2D>();
        Animator = gameObject.GetComponent<Animator>();
        Collider = gameObject.GetComponent<BoxCollider2D>();

        startScaleX = transform.localScale.x;
        inCollision = false;

        Standing = true;
        IsMoved = false;
        Heading = Direction.Down;

        Inventory.OwnItem("Phaser");
    }

    protected virtual void Update()
    {
    }

    protected virtual void FixedUpdate()
    {
    }


    /*** Movement ***/

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

    protected void WalkCR(Direction direction, float distance, float speed, simpleFunc atEnd)
    {
        Standing = false;
        Heading = direction;
        UpdateAnimation();
        StartCoroutine(moveCR(HeadingVector, distance, speed, atEnd));
    }

    private IEnumerator moveCR(Vector3 dirVector, float distance, float speed, simpleFunc atEnd)
    {
        if (dirVector.sqrMagnitude == 0)
            yield break;

        float sqrRemainingDistance;
        Vector3 endPosition = transform.position + (dirVector * distance);
        IsMoved = true;

        do
        {
            Vector2 updatePosition = Vector3.MoveTowards(Rb2d.position, endPosition, speed * Time.deltaTime);
            Rb2d.MovePosition(updatePosition);
            sqrRemainingDistance = (transform.position - endPosition).sqrMagnitude;

            yield return null;
        }
        while (!inCollision && sqrRemainingDistance > float.Epsilon);

        if (atEnd != null)
            atEnd();

        IsMoved = false;
    }

    protected void MoveTime(Vector3 dirVector, float duration, float speed)
    {
        Walk(Direction.None); // To stop walking animation + flags
        IsMoved = true;
        Rb2d.velocity = dirVector * speed;
        StartCoroutine(Timer(duration, moveTimeEnded));
    }

    private void moveTimeEnded()
    {
        Rb2d.velocity = Vector2.zero;
        IsMoved = false;
    }


    /*** Events ***/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        inCollision = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        inCollision = false;
    }

    public virtual void OnHit()
    {
        Life--;
        if (Life <= 0)
            Die();
    }


    /*** Others ***/

    protected virtual void UpdateAnimation()
    {
        Animator.SetInteger("moveX", (Standing ? 0 : (int)HeadingVector.x));
        Animator.SetInteger("moveY", (Standing ? 0 : (int)HeadingVector.y));
        Animator.SetBool("standing", Standing);

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

    protected IEnumerator Timer(float duration, simpleFunc atEnd)
    {
        float start = Time.time;

        while (Time.time < (start + duration))
            yield return null;

        if (atEnd != null)
            atEnd();
    }

    protected virtual void Die()
    {
        Debug.Log("Aww, " + gameObject.name + " died!");
        //throw new NotImplementedException();
    }

    internal void Shoot()
    {
        Vector3 shotPosition = gameObject.transform.position + (HeadingVector * (Collider.size.x + ShotCtrl.Collider.size.x) / 2);
        Quaternion headingRotation = Quaternion.FromToRotation(gameObject.transform.position, shotPosition);
        //Quaternion headingRotation = Quaternion.LookRotation(Vector3.zero, HeadingVector);
        GameObject shot = Instantiate(Shot, shotPosition, headingRotation);
        Rigidbody2D shotRb2d = shot.GetComponent<Rigidbody2D>();
        shotRb2d.velocity = HeadingVector * ShotCtrl.Speed;
    }
}
