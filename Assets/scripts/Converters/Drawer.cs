using Assets.scripts.UIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Assets.scripts.Converters
{
	public class Drawer : IDrawer
	{
		public Drawer(Text _textBox)
		{
			textBox = _textBox;
		}
		private readonly Text textBox;
		public void PrintError(string text)
		{
			textBox.text = text;
		}
	}
}
