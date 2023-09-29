using UnityEditor;
using UnityEngine;

public class MenuItemEditorSystem
{
    [MenuItem("MoleculeGenerator/Create Chemical Element Color Config")]
    public static void CreateChemicalElementColorConfig()
    {
        var config = ScriptableObject.CreateInstance<ChemicalElementColorConfig>();

        string directoryPath = "Assets/Data";
        if (!System.IO.Directory.Exists(directoryPath))
        {
            System.IO.Directory.CreateDirectory(directoryPath);
        }

        string assetPath = "Assets/Data/ChemicalElementColorConfig.asset";

        AssetDatabase.CreateAsset(config, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
