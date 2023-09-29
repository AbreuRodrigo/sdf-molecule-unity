using System;
using System.Collections.Generic;

[Serializable]
public class Molecule
{
    public string Name { get; set; }
    public List<Atom> Atoms { get; set; }
    public List<Bond> Bonds { get; set; }
}