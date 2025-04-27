using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class FindBrokenMaterials
{
    [MenuItem("Tools/Select All Broken or Standard Materials")]
    static void SelectBrokenOrStandardMaterials()
    {
        string[] allMaterialGUIDs = AssetDatabase.FindAssets("t:Material");
        List<Object> brokenMaterials = new List<Object>();

        foreach (string guid in allMaterialGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat != null && mat.shader != null)
            {
                string shaderName = mat.shader.name;
                if (shaderName == "Hidden/InternalErrorShader" || shaderName == "Standard")
                {
                    brokenMaterials.Add(mat);
                }
            }
        }

        Selection.objects = brokenMaterials.ToArray();

        Debug.Log($"Selected {brokenMaterials.Count} broken or Standard materials.");
    }
}
