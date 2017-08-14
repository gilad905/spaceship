using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float Speed;
    public enum Direction { None, Up, Right, Down, Left };
    public short Life;
    private GameObject shotInstance;

    protected delegate void simpleFunc();
    protected GameObject InteractingWith = null;
    protected Rigidbody2D Rb2d;
    protected Animator Animator;
    protected BoxCollider2D Collider;
    protected Vector3 HeadingVector;
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
    protected const float StepLength = 1.2F;
    protected bool Standing { get; private set; }
    protected bool IsMoved;
    protected float ColliderHalfSize;

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
        shotInstance = GameObject.FindGameObjectWithTag("Shot");

        Standing = true;
        IsMoved = false;
        Heading = Direction.Down;
        ColliderHalfSize = Math.Max(Collider.size.x, Collider.size.y) / 2;
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

            Rb2d.velocity = HeadingVector * Speed;
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

    protected static Vector3 GetDirectionVector(Direction direction)
    {
        #pragma warning disable IDE0017 // Simplify object initialization
        Vector3 result = new Vector3();
        #pragma warning restore IDE0017 // Simplify object initialization

        result.x = (direction == Direction.Right ? 1 : (direction == Direction.Left ? -1 : 0));
        if (result.x == 0)
            result.y = (direction == Direction.Up ? 1 : (direction == Direction.Down ? -1 : 0));

        return result;
    }

    protected virtual void UpdateAnimation()
    {
        Animator.SetInteger("moveX", (Standing ? 0 : (int)HeadingVector.x));
        Animator.SetInteger("moveY", (Standing ? 0 : (int)HeadingVector.y));
        Animator.SetBool("standing", Standing);

        if (HeadingVector.x != 0)
            transform.localScale = new Vector3(startScaleX * HeadingVector.x, transform.localScale.y, transform.localScale.z);
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
        Shot shotCtrl = (Shot)shotInstance.GetComponent<Shot>();
        Vector3 shotStart = transform.position + (HeadingVector * (ColliderHalfSize + shotCtrl.ColliderHalfSize));
        GameObject shot = Instantiate(shotInstance);

        shot.transform.position = shotStart;
        shot.transform.rotation = rotationFromDirection(HeadingVector - Vector3.zero);
        Rigidbody2D shotRb2d = shot.GetComponent<Rigidbody2D>();
        shotRb2d.velocity = HeadingVector * Shot.Speed;
    }

    private Quaternion rotationFromDirection(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
