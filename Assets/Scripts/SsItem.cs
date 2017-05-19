using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SsItem
{
    public string Name;
    public delegate void UseFunc();
    public UseFunc Use;

    public SsItem(string name, UseFunc use)
    {
        this.Name = name;
        this.Use = use;
    }
}