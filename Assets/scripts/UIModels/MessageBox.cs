using Assets.scripts.Exstensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts.UIModels
{
	public class MessageBox
	{
		public MessageBox(IDrawer _drawer)
		{
			drawer = _drawer;
			Text = new StringBuilder(string.Empty);
			InvalidClick += ShowError;
		}
		public event Action InvalidClick;
		public StringBuilder Text { get; private set; }

		private readonly IDrawer drawer;
		private static readonly List<char> tabooInputSigns = new List<char>() { '.', ',', ' ', '\'', '\"' };
		public void PrintSyntaxError()
		{
			drawer.PrintError($"dont use signs these signs ({tabooInputSigns.ListToString<char>()})");
		}
		public bool CheckSyntax(string name)
		{
			foreach (var item in tabooInputSigns)
				if (name.Contains(item))
					return true;

			return false;
		}
		public void ChangeText(string text)
		{
			this.Text = new StringBuilder(text);
		}
		private void ShowError()
		{
			drawer.ShowAnimation(true);
		}
		public void BackConditional()
		{
			drawer.ShowAnimation(false);
		}
		public void InvokeInvalidClick()
		{
			if (InvalidClick != null)
				InvalidClick.Invoke();
		}
		public void ClearText()
		{
			drawer.ClearText();
		}
	}
}
