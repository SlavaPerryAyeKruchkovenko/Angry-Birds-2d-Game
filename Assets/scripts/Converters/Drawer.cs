using Assets.scripts.UIModels;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.Converters
{
	public class Drawer : IDrawer
	{
		public Drawer(Text _textBox, GameObject _gameobject)
		{
			textBox = _textBox;
			gameObject = _gameobject;
		}
		private readonly Text textBox;
		private readonly GameObject gameObject;
		public void PrintError(string text)
		{
			if (textBox)
				textBox.text = text;
		}

		public void ShowAnimation(bool value)
		{
			var animator = gameObject.GetComponent<Animator>();
			if (animator)
				animator.SetBool("HaveError", value);
		}

		public void ClearText()
		{
			textBox.text = string.Empty;
		}
	}
}
