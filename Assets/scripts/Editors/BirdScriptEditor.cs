using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(BirdScript))]
public class BirdScriptEditor : Editor
{
	BirdScript script;
	public void OnEnable()
	{
		script = (BirdScript)target;
	}
	public override void OnInspectorGUI()
	{
		if (script.FlyMaterial == null)
		{
			EditorGUILayout.LabelField("Please add Material", EditorStyles.boldLabel);
		}
		script.FlyMaterial = (GameObject)EditorGUILayout.ObjectField($"points", script.FlyMaterial, typeof(GameObject), true);
	}
	public static void SetObjectDirty(GameObject obj)
	{
		EditorUtility.SetDirty(obj);
		EditorSceneManager.MarkSceneDirty(obj.scene);
	}
}
