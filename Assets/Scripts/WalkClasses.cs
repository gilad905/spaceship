using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkPath
{
    public int Amount { get; set; }
    public Character.Direction Direction { get; set; }

    public WalkPath(Character.Direction direction, int amount)
    {
        this.Direction = direction;
        this.Amount = amount;
    }
}

public class WalkRoute
{
    public List<WalkPath> Paths = new List<WalkPath>();
    public bool IsLoop;
}

