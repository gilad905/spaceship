using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{
    protected override void Start()
    {
        Route.Paths = new List<WalkPath>() {
            new WalkPath(Direction.None, 3),
            new WalkPath(Direction.Left, 5),
            new WalkPath(Direction.Up, 2),
            new WalkPath(Direction.None, 3),
            new WalkPath(Direction.Right, 5),
            new WalkPath(Direction.Down, 2),
        };
        Route.IsLoop = true;
        base.Start();
    }
}
