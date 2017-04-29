using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public float speed;

    Rigidbody2D rb2d = null;
    Animator animator = null;
    BoxCollider2D interactionCollider = null;
    Vector3 startScale;

    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        interactionCollider = gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>();
        startScale = transform.localScale;
    }

    void FixedUpdate()
    {
        move();
    }

    void move()
    {
        int moveHor = 0, moveVer = 0;
        moveHor = (Input.GetKey(KeyCode.RightArrow) ? 1 : (Input.GetKey(KeyCode.LeftArrow) ? -1 : 0));
        if (moveHor == 0)
            moveVer = (Input.GetKey(KeyCode.UpArrow) ? 1 : (Input.GetKey(KeyCode.DownArrow) ? -1 : 0));

        Vector2 movement = new Vector2(moveHor, moveVer);
        rb2d.velocity = movement * speed;

        if (moveHor != 0)
            transform.localScale = new Vector3(startScale.x * moveHor, startScale.y, startScale.z);

        animator.SetInteger("moveHor", moveHor);
        animator.SetInteger("moveVer", moveVer);
        animator.SetBool("idle", (moveHor == 0 && moveVer == 0));

        rotateInteractionCollider(moveHor, moveVer);
    }

    void rotateInteractionCollider(int moveHor, int moveVer)
    {
        if (interactionCollider != null && (moveHor != 0 || moveVer != 0))
        {
            int angle = moveHor != 0 ? moveHor * 90 : (moveVer > 0 ? -180 : 0);
            interactionCollider.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private void OnInteractionEnter(Collider2D collider)
    {
    }

    private void OnInteractionExit(Collider2D collider)
    {
    }
}
