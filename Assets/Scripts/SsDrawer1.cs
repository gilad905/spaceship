﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SsDrawer1 : SsObject
{
    bool isClosed = true;
    Sprite closedSprite = null;
    public Sprite OpenSprite = null;

    public override void Interact()
    {
        if (isClosed)
        {
            SR.sprite = OpenSprite;
            Inventory.OwnItem("phaser");
        }
        else
            SR.sprite = closedSprite;
        isClosed = !isClosed;
    }

    public override void Start()
    {
        base.Start();
        closedSprite = SR.sprite;
    }
}