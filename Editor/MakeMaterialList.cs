using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakeMaterialList {
	[MenuItem("Voxel/Create/MaterialList")]
	public static void CreateMyAsset()
	{
		MaterialList asset = ScriptableObject.CreateInstance<MaterialList>();

		AssetDatabase.CreateAsset(asset, "Assets/NewMaterialList.asset");
		AssetDatabase.SaveAssets();

		EditorUtility.FocusProjectWindow();

		Selection.activeObject = asset;
	}
}