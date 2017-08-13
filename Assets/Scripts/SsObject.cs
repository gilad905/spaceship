using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SsObject : MonoBehaviour, IInteractable
{
    protected MeshRenderer MR;
    protected MeshFilter MF;
    protected BoxCollider2D BC;
    protected SpriteRenderer SR;

    void OnCollisionEnter2D(Collision2D other)
    {
        collideIfNeeded(other.gameObject, true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        collideIfNeeded(other.gameObject, true);
    }

    void OnCollisionExit2D(Collision2D other)
    {
        collideIfNeeded(other.gameObject, false);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        collideIfNeeded(other.gameObject, false);
    }

    void collideIfNeeded(GameObject other, bool isEnter)
    {
        if (other.name != "InteractionCollider")
        {
            if (isEnter)
                CollideEnter(other);
            else
                CollideExit(other);
        }
    }

    public virtual void CollideEnter(GameObject other)
    {
    }

    public virtual void CollideExit(GameObject other)
    {
    }

    [ExecuteInEditMode]
    public virtual void SetProperties(IDictionary<string, string> props)
    {

    }
    
    public void Hide()
    {
        if (MR != null)
            MR.enabled = false;
        else
            SR.enabled = false;
    }

    public void Show()
    {
        if (MR != null)
            MR.enabled = true;
        else
            SR.enabled = true;
    }

    protected virtual void Start()
    {
        BC = gameObject.GetComponent<BoxCollider2D>();
        MR = gameObject.GetComponent<MeshRenderer>();
        MF = gameObject.GetComponent<MeshFilter>();
        SR = gameObject.GetComponent<SpriteRenderer>();
    }

    public virtual void Interact()
    {

    }
}