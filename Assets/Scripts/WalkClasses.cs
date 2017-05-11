using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkPath
{
    public int Amount { get; set; }
    public Character.Direction Direction { get; set; }

    public WalkPath(Character.Direction _direction, int _amount)
    {
        this.Direction = _direction;
        this.Amount = _amount;
    }
}

public class WalkRoute
{
    public List<WalkPath> Paths;
    public bool IsLoop;
}

