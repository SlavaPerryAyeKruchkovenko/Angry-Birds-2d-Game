/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

namespace Assets.scripts
{
	public enum Birds
	{
		Red, Blue, Yellow, Black, Green, White, BigRed
	}
	public class Bird: TypeOfGameObject
	{
		public override float Health { get; protected set; } = 1;
		public override float Armor { get; protected set; } = 0;

		public override short SpriteCoount => 2;
		public static Bird GetBird(Birds bird)
		{
			return bird switch
			{
				Birds.Red => new RedBird(),
				Birds.Blue => new BlueBird(),
				Birds.Yellow => new YellowBird(),
				Birds.Black => new BlackBird(),
				Birds.Green => new GreenBird(),
				Birds.White => new WhiteBird(),
				Birds.BigRed => new BigRedBird(),
				_ => throw new Exception("Bird not found"),
			};
		}
	}
	public class RedBird : Bird
	{
	}
	public class BlueBird : Bird
	{

	}
	public class YellowBird : Bird
	{

	}
	public class BigRedBird : Bird
	{

	}
	public class BlackBird : Bird
	{

	}
	public class WhiteBird : Bird
	{

	}
	public class GreenBird : Bird
	{

	}

}
*/