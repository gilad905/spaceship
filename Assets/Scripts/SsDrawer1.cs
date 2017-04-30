using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SsDrawer1 : SsObject
{
    bool isClosed = true;
    Mesh closedMesh = null;
    //Mesh openMesh = null;

    public override void Interact()
    {
        if (isClosed)
            open();
        else
            close();
        isClosed = !isClosed;
    }

    private void close()
    {
    }

    private void open()
    {
        Inventory.OwnItem("phaser");
    }

    public override void Start()
    {
        base.Start();
        closedMesh = meshFilter.sharedMesh;
        //openMesh = (MeshFilter)
    }
}