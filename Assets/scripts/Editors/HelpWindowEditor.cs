using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

class HelpWindowEditor : EditorWindow
{
	[MenuItem("Tools/All Conditionals")]	
	public static void ShowWindow()
	{
		GetWindow(typeof(HelpWindowEditor));
	}
	private Vector2 scrollPos;
	private void OnGUI()
	{
		EditorGUILayout.BeginVertical();
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos,GUILayout.Width(position.width), GUILayout.Height(position.height));
		GetConditionalAboutTag("Pig");
		GetConditionalAboutTag("Build Material");
		Repaint();
		EditorGUILayout.EndScrollView();
	}
	private static void GetConditionalAboutTag(string tag)
	{
		var birds = GameObject.FindGameObjectsWithTag(tag);
		if (birds != null)
		{
			foreach (var item in birds)
			{
				EditorGUILayout.LabelField(item.name, EditorStyles.boldLabel);
				if(item.GetComponent<GameObjectScript>().ABGameObj!= null)
				{
					EditorGUILayout.LabelField(item.GetComponent<GameObjectScript>().ABGameObj.Health.ToString());
					EditorGUILayout.LabelField(item.GetComponent<GameObjectScript>().ABGameObj.Armor.ToString());
				}			
				EditorGUILayout.Space();
			}
		}
	}
}
