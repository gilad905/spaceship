using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ssDoor : ssObject {
    public bool isLocked = true;
    bool isClosed = true;

    public override void setProperties(IDictionary<string, string> props)
    {
        if (props.ContainsKey("isLocked"))
            isLocked = (props["isLocked"] == "true");
    }

    public override void collideEnter(GameObject other)
    {
        if (!isLocked && other.name == "Player")
        {
            isClosed = false;
            hide();
        }
    }

    public override void collideExit(GameObject other)
    {
        if (!isClosed)
        {
            isClosed = true;
            show();
        }
    }
}
