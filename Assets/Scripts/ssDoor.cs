using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SsDoor : SsObject {
    public bool isLocked;
    bool isClosed = true;

    protected override void Start()
    {
        base.Start();
        if (BC != null)
            BC.isTrigger = !isLocked;
    }

    public override void SetProperties(IDictionary<string, string> props)
    {
        if (props.ContainsKey("isLocked"))
            isLocked = (props["isLocked"] == "true");
    }

    public override void CollideEnter(GameObject other)
    {
        if (!isLocked && other.tag == "Player")
        {
            isClosed = false;
            Hide();
        }
    }

    public override void CollideExit(GameObject other)
    {
        if (!isClosed)
        {
            isClosed = true;
            Show();
        }
    }

    public void Lock()
    {
        isLocked = true;
        if (BC != null)
            BC.isTrigger = false;
    }

    public void Unlock()
    {
        isLocked = false;
        if (BC != null)
            BC.isTrigger = true;
    }
}
