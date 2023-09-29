using System.Collections.Generic;
using UnityEngine;

public class Molecule3DGenerator
{
    private ObjectSpawner m_objectSpawner;
    
    public Molecule3DGenerator(ObjectSpawner objectSpawner)
    {
        m_objectSpawner = objectSpawner;
    } 
    
    public GameObject Generate3DMolecule(string sdfMoleculeString)
    {
        var molecules = SDFParser.ParseFromString(sdfMoleculeString);
        var atomObjects = new Dictionary<int, GameObject>();

        foreach (var molecule in molecules)
        {
            var moleculeObj = new GameObject($"Molecule{molecule.Name}");

            foreach (var atom in molecule.Atoms)
            {
                var atomObj = m_objectSpawner.InstantiateAtom(atom, moleculeObj.transform);
                atomObjects[atom.Index] = atomObj;
            }

            foreach (var bond in molecule.Bonds)
            {
                if (!atomObjects.ContainsKey(bond.Begin) || !atomObjects.ContainsKey(bond.End))
                {
                    continue;
                }

                var beginAtom = atomObjects[bond.Begin].transform.position;
                var endAtom = atomObjects[bond.End].transform.position;
                m_objectSpawner.InstantiateBond(bond, moleculeObj.transform, beginAtom, endAtom);
            }

            return moleculeObj;
        }

        return null;
    }
}