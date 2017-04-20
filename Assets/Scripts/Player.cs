using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public float speed;

	private Rigidbody2D rb2d;
	private Animator animator;
	private Vector3 startScale;

	void Start ()
	{
		rb2d = gameObject.GetComponent<Rigidbody2D> ();
		animator = gameObject.GetComponent<Animator> ();
		startScale = transform.localScale;
	}

	void FixedUpdate ()
	{
		Move ();
	}

	private void Move ()
	{
		int moveHor = 0, moveVer = 0;
		moveHor = (Input.GetKey (KeyCode.RightArrow) ? 1 : (Input.GetKey (KeyCode.LeftArrow) ? -1 : 0));
		if (moveHor == 0)
			moveVer = (Input.GetKey (KeyCode.UpArrow) ? 1 : (Input.GetKey (KeyCode.DownArrow) ? -1 : 0));

		Vector2 movement = new Vector2 (moveHor, moveVer);
		rb2d.velocity = movement * speed;

		animator.SetInteger ("moveHor", moveHor);
		animator.SetInteger ("moveVer", moveVer);
		if (moveHor != 0)
			transform.localScale = new Vector3 (startScale.x * moveHor, startScale.y, startScale.z);
	}
}
