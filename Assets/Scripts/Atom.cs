using System;
using UnityEngine;

[Serializable]
public class Atom
{
    public int Index { get; set; }
    public string Symbol { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
}