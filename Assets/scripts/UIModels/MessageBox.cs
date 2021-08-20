using Assets.scripts.Exstensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts.UIModels
{
	public class MessageBox : Object
	{
		public MessageBox(GameObject _gameObject, IDrawer _drawer)
		{
			GameObject = _gameObject;
			drawer = _drawer;
			text = new StringBuilder(string.Empty);
		}
		private readonly IDrawer drawer;
		public StringBuilder text { get; private set; }
		public GameObject GameObject { get; }
		private static readonly List<char> tabooSigns = new List<char>() { '.', ',', ' ', '\'', '\"' };
		public void PrintSyntaxError()
		{
			drawer.PrintError($"dont use signs these signs ({tabooSigns.ListToString<char>()})");
		}
		public bool CheckSyntax(string name)
		{
			foreach (var item in tabooSigns)
				if (name.Contains(item))
					return true;

			return false;
		}
		public void ChangeText(string text)
		{
			this.text = new StringBuilder(text);
		}
	}
}
