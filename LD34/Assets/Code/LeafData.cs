using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

public class LeafData : ScriptableObject
{
    public Mesh mesh;
    public Material material;
    public List<Vector3> childPoints;

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Leaf Data")]
    public static void Create()
    {
        LeafData data = ScriptableObject.CreateInstance<LeafData>();
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (File.Exists(path))
        {
            path = path.Replace("/" + Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(LeafData).Name + ".asset");
        AssetDatabase.CreateAsset(data, assetPathAndName);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = data;



    }
#endif
}
