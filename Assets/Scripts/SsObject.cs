using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ssObject : MonoBehaviour
{
    protected MeshRenderer meshRenderer;
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
                collideEnter(other);
            else
                collideExit(other);
        }
    }

    public virtual void collideEnter(GameObject other)
    {
    }

    public virtual void collideExit(GameObject other)
    {
    }

    public virtual void setProperties(IDictionary<string, string> props)
    {

    }
    
    public void hide()
    {
        meshRenderer.material = null;
        if (!bcIsTrigger)
            bc.isTrigger = true;
    }

    public void show()
    {
        meshRenderer.material = material;
        if (!bcIsTrigger)
            bc.isTrigger = false;
    }

    void Start()
    {
        bc = gameObject.GetComponent<BoxCollider2D>();
        bcIsTrigger = bc.isTrigger;
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        material = meshRenderer.material;
    }
}