using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public GameObject Player;

	void LateUpdate ()
	{
		stickToPlayerPosition ();
	}

	void stickToPlayerPosition ()
	{
		var playerPos = Player.transform.position;
		transform.position = new Vector3 (playerPos.x, playerPos.y, transform.position.z);
	}
}