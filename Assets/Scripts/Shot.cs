using System;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public static float Speed = 10;
    public static BoxCollider2D Collider = null;

    public float ColliderHalfSize { get; set; }

    private void Start()
    {
        if (Collider == null)
            Collider = gameObject.GetComponent<BoxCollider2D>();

        ColliderHalfSize = Math.Max(Collider.size.x, Collider.size.y) / 2;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Character otherCtrl = (Character)collision.gameObject.GetComponent<Character>();
        if (otherCtrl != null)
            otherCtrl.OnHit();
        Destroy(gameObject);
    }
}
