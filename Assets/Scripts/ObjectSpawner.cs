using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner
{
    private GameObject m_largeSpherePrefab;
    private GameObject m_smallSpherePrefab;
    private GameObject m_bondPrefab;

    private Dictionary<string, Material> m_elementMaterials = new Dictionary<string, Material>();
    private Dictionary<string, Color> m_elementColors = new Dictionary<string, Color>();

    public ObjectSpawner(ChemicalElementColorConfig config, GameObject largeSpherePrefab, GameObject smallSpherePrefab, GameObject bondPrefab)
    {
        m_largeSpherePrefab = largeSpherePrefab;
        m_smallSpherePrefab = smallSpherePrefab;
        m_bondPrefab = bondPrefab;

        foreach (ChemicalElementColor chemicalElementColor in config.m_chemicalElementColorConfigs)
        {
            m_elementColors[chemicalElementColor.m_elementName] = chemicalElementColor.m_elementColor;
        }
    }
    
    public GameObject InstantiateAtom(Atom atom, Transform parent)
    {
        GameObject atomObj = null;
        
        if (atom.Symbol.Equals("C"))
        {
            atomObj = Object.Instantiate(m_largeSpherePrefab, parent);
        }
        else
        {
            atomObj = Object.Instantiate(m_smallSpherePrefab, parent);
        }

        if (atomObj == null)
        {
            return atomObj;
        }
        
        atomObj.name = $"Atom_{atom.Symbol}_{atom.Index}"; 
        atomObj.transform.position = new Vector3((float)atom.X, (float)atom.Y, (float)atom.Z);
        
        Renderer objRenderer = atomObj.GetComponent<Renderer>();

        if (m_elementMaterials.TryGetValue(atom.Symbol, out Material material))
        {
            objRenderer.material = material;
        }
        else
        {
            Material newMaterial = new Material(Shader.Find("Standard"));
            
            if (m_elementColors.TryGetValue(atom.Symbol, out Color color)) {
                newMaterial.color = color;
                objRenderer.material = newMaterial;
            }
            else
            {
                Debug.LogWarning($"Color for {atom.Symbol} not found in the config.");
            }
        }

        return atomObj;
    }
    
    public void InstantiateBond(Bond bond, Transform parent, Vector3 atomAPosition, Vector3 atomBPosition)
    {
        var bondPosition = (atomAPosition + atomBPosition) / 2f;
        var bondObj = Object.Instantiate(m_bondPrefab, bondPosition, Quaternion.identity, parent);
        bondObj.name = $"Bond_{bond.Begin}x{bond.End}";
                
        var bondScale = Vector3.Distance(atomAPosition, atomBPosition);
        bondObj.transform.localScale = new Vector3(0.05f, bondScale / 2f, 0.05f);
                
        var direction = atomBPosition - atomAPosition;
        var rotation = Quaternion.FromToRotation(Vector3.up, direction.normalized);
        bondObj.transform.rotation = rotation;
    }
}