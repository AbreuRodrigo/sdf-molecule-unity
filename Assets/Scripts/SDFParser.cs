using System;
using System.Collections.Generic;
using System.IO;

public static class SDFParser
{
    public static List<Molecule> ParseFromString(string sdfData)
    {
        var molecules = new List<Molecule>();
        Molecule currentMolecule = null;

        using (var reader = new StringReader(sdfData))
        {
            var atomCounter = 0;

            while (reader.ReadLine() is { } line)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.StartsWith("$$$$"))
                {
                    if (currentMolecule != null)
                    {
                        molecules.Add(currentMolecule);
                        currentMolecule = null;
                    }
                }
                else if (currentMolecule == null)
                {
                    currentMolecule = new Molecule
                    {
                        Name = line.Trim(),
                        Atoms = new List<Atom>(),
                        Bonds = new List<Bond>()
                    };
                }
                else
                {
                    var fields = line.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (fields.Length >= 16)
                    {
                        var atom = new Atom
                        {
                            Index = ++atomCounter,
                            Symbol = fields[3],
                            X = double.Parse(fields[0]),
                            Y = double.Parse(fields[1]),
                            Z = double.Parse(fields[2])
                        };
                        currentMolecule.Atoms.Add(atom);
                    }
                    else if (currentMolecule.Atoms.Count > 0 && fields.Length >= 6 && !fields[0].Equals("M"))
                    {
                        var bond = new Bond
                        {
                            Begin = int.Parse(fields[0]),
                            End = int.Parse(fields[1])
                        };
                        currentMolecule.Bonds.Add(bond);
                    }
                }
            }
        }

        if (currentMolecule != null)
        {
            molecules.Add(currentMolecule);
        }

        return molecules;
    }
}