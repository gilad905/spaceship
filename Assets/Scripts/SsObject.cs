using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SsObject : MonoBehaviour
{
    protected MeshRenderer meshRenderer;
    protected MeshFilter meshFilter;
    protected BoxCollider2D bc;
    Material material;
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
        meshRenderer.material = null;
        if (!bcIsTrigger)
            bc.isTrigger = true;
    }

    public void Show()
    {
        meshRenderer.material = material;
        if (!bcIsTrigger)
            bc.isTrigger = false;
    }

    public virtual void Start()
    {
        bc = gameObject.GetComponent<BoxCollider2D>();
        bcIsTrigger = bc.isTrigger;
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshFilter = gameObject.GetComponent<MeshFilter>();
        material = meshRenderer.material;
    }

    public virtual void Interact()
    {

    }
}