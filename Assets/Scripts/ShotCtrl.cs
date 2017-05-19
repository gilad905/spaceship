using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotCtrl : MonoBehaviour
{
    public static float Speed = 2;
    public static BoxCollider2D Collider = null;

    private void Start()
    {
        if (Collider == null)
            Collider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            string otherCtrlName = other.name.Replace(" ", "");
            Character otherCtrl = (Character)other.GetComponent(otherCtrlName);
            otherCtrl.OnHit();
        }
        Destroy(gameObject);
    }
}
