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
	private void OnGUI()
	{
		
		GetConditionalAboutTag("Pig");
		GetConditionalAboutTag("Build Material");
		this.Repaint();
	}
	private static void GetConditionalAboutTag(string tag)
	{
		var birds = GameObject.FindGameObjectsWithTag(tag);
		if (birds != null)
		{
			foreach (var item in birds)
			{
				EditorGUILayout.LabelField(item.name, EditorStyles.boldLabel);
				EditorGUILayout.LabelField(item.GetComponent<GameObjectScript>().Type.Health.ToString());
				EditorGUILayout.LabelField(item.GetComponent<GameObjectScript>().Type.Armor.ToString());
				EditorGUILayout.Space();
			}
		}
	}
}
