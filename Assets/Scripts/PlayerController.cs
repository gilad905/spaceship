using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	public float speed;

	private Rigidbody2D rb2d;

	void Start ()
	{
		rb2d = gameObject.GetComponent<Rigidbody2D> ();

	}

	void FixedUpdate ()
	{
		move ();
	}

	private void move ()
	{
		int moveHor = 0, moveVer = 0;
		moveHor = (Input.GetKey(KeyCode.RightArrow) ? 1 : (Input.GetKey(KeyCode.LeftArrow) ? -1 : 0));
		if (moveHor == 0)
			moveVer = (Input.GetKey(KeyCode.UpArrow) ? 1 : (Input.GetKey(KeyCode.DownArrow) ? -1 : 0));

		Vector2 movement = new Vector2 (moveHor, moveVer);
		rb2d.velocity = movement * speed;
	}
}
