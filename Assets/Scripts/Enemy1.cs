using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{
    protected override void Start()
    {
        route.Paths = new List<WalkPath>() {
            new WalkPath(Direction.Left, 5),
            new WalkPath(Direction.Up, 2),
            new WalkPath(Direction.Right, 5),
            new WalkPath(Direction.Down, 2),
        };
        route.IsLoop = true;
        base.Start();
    }
}
