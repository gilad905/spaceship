using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SsDoor : SsObject {
    public bool isLocked = true;
    bool isClosed = true;

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
}
