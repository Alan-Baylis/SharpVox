using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialList : ScriptableObject {
	public Material[] materials;
	public Material errorMaterial;
	public Material getFromBlockId(int id) {
		if (id >= materials.Length || materials[id] == null) {
			Debug.LogError ("No material in Material List for id: " + id);
			return errorMaterial;
		}
		return materials [id];
	}
}
