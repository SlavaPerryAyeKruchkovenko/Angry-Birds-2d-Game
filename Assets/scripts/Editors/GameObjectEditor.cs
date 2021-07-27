using Assets.scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
[CustomEditor(typeof(GameObjectScript))]
public class GameObjectEditor : Editor
{
	GameObjectScript script;
	public void OnEnable()
	{
		script = (GameObjectScript)this.target;
	}
	public override void OnInspectorGUI()
	{
		script.TypeOfGameObj = TypeOfGameObject.GetTypesOfGameObject(script.gameObject);
		switch (script.TypeOfGameObj)
		{
			case TypesOfGameObject.Bird: script.BirdType = (Birds)EditorGUILayout.EnumPopup("Bird type", script.BirdType); break;
			case TypesOfGameObject.Pig: script.PigType = (Pigs)EditorGUILayout.EnumPopup("Pig type", script.PigType); break;
			case TypesOfGameObject.BuildMaterial: script.MaterialType = (BuildMaterials)EditorGUILayout.EnumPopup("Material type", script.MaterialType); break;
			default:
				break;
		}
		script.Start();		
		if(GUILayout.Button("Add new Sprite", GUILayout.Height(20)))
		{			
			if(script.ConditionalSprites == null)
			{
				script.ConditionalSprites = new List<Sprite>();
			}
			script.ConditionalSprites.Add(Sprite.Create(null,default,default));
		}		
		if(script.ConditionalSprites.Count>0)
		{
			for (int i = 0; i < script.ConditionalSprites.Count; i++)
			{
				script.ConditionalSprites[i] = (Sprite)EditorGUILayout.ObjectField(
					$"Image {i + 1}", script.ConditionalSprites[i], typeof(Sprite), true);
			}
		}
		if (GUILayout.Button("Delete last Sprite", GUILayout.Height(20)))
		{
			script.ConditionalSprites.RemoveAt(script.ConditionalSprites.Count - 1);
		}
		if (script.ConditionalSprites.Count != script.Type.SpriteCoount)
		{
			var notHave = script.Type.SpriteCoount - script.ConditionalSprites.Count;
			EditorGUILayout.LabelField($"Нехватает {notHave} Спрайтов", EditorStyles.boldLabel);
		}
	}
	public static void SetObjectDirty(GameObject obj)
	{
		EditorUtility.SetDirty(obj);
		EditorSceneManager.MarkSceneDirty(obj.scene);
	}
}
 