using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SsObject : MonoBehaviour
{
    protected MeshRenderer MR;
    protected MeshFilter MF;
    protected BoxCollider2D BC;
    protected SpriteRenderer SR;
    int sortingLayerID;
    bool bcIsTrigger;

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
            MR.sortingLayerName = null;
        else
            SR.sortingLayerName = null;
        if (!bcIsTrigger)
            BC.isTrigger = true;
    }

    public void Show()
    {
        if (MR != null)
            MR.sortingLayerID = sortingLayerID;
        else
            SR.sortingLayerID = sortingLayerID;
        if (!bcIsTrigger)
            BC.isTrigger = false;
    }

    public virtual void Start()
    {
        BC = gameObject.GetComponent<BoxCollider2D>();
        bcIsTrigger = BC.isTrigger;
        MR = gameObject.GetComponent<MeshRenderer>();
        MF = gameObject.GetComponent<MeshFilter>();
        SR = gameObject.GetComponent<SpriteRenderer>();
        sortingLayerID = (MR != null) ? MR.sortingLayerID : SR.sortingLayerID;
    }

    public virtual void Interact()
    {

    }
}