using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts
{
	public enum Pigs
	{
		StandartPig, BigPig, ArmorPig, KingPig
	}
	public class Pig : TypeOfGameObject
	{
		public override float Health { get; protected set; }
		public override float Armor { get; protected set; }

		public override short SpriteCoount => 3;
		public static Pig GetPig(Pigs pig) => pig switch
		{
			Pigs.ArmorPig => new ArmorPig(),
			Pigs.BigPig => new BigPig(),
			Pigs.KingPig => new KingPig(),
			Pigs.StandartPig => new StandartPig(),
			_ => throw new NotImplementedException()
		};
	}
	public class StandartPig : Pig
	{
		public override float Health { get; protected set; } = 100;
		public override float Armor { get; protected set; } = 0;
	}
	public class BigPig : Pig
	{
		public override float Health { get; protected set; } = 100;
		public override float Armor { get; protected set; } = 200;
		public override float Weight => 2;
	}
	public class ArmorPig : Pig
	{
		public override float Health { get; protected set; } = 100;
		public override float Armor { get; protected set; } = 100;
		public override float Weight => 1.2f;
	}
	public class KingPig : Pig
	{
		public override float Health { get; protected set; } = 100;
		public override float Armor { get; protected set; } = 500;
		public override float Weight => 2.5f;
	}
}
