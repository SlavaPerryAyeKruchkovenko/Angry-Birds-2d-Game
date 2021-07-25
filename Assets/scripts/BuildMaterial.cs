using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts
{
	public enum TypeOfBuildMaterial
	{
		Ice, Wood, Stone
	}
	public abstract class BuildMaterial
	{
		public abstract float Health { get; protected set; }
		public abstract float Armor { get; protected set; }
		public abstract float Weight { get; }
		public static BuildMaterial GetBuildMaterial(TypeOfBuildMaterial material) => material switch
		{
			TypeOfBuildMaterial.Ice => new IceBuildMaterial(),
			TypeOfBuildMaterial.Wood => new WoodBildMaterial(),
			TypeOfBuildMaterial.Stone => new StoneBildMaterial(),
			_ => throw new NotImplementedException()
		};
		public void GetDamage(float damage)
		{
			if (damage > 1)
			{
				Armor -= 0.666f * damage;
				Health -= 0.333f * damage;
				if (Armor < 0)
				{
					Health -= Math.Abs(Armor);
					Armor = 0;
				}
			}
		}
	}
	public class IceBuildMaterial : BuildMaterial
	{
		public override float Health { get; protected set; } = 100;

		public override float Armor { get; protected set; } = 0;

		public override float Weight => 0.5f;
	}
	public class WoodBildMaterial : BuildMaterial
	{
		public override float Health { get; protected set; } = 100;

		public override float Armor { get; protected set; } = 100;

		public override float Weight => 1f;
	}
	public class StoneBildMaterial : BuildMaterial
	{
		public override float Health { get; protected set; } = 100;

		public override float Armor { get; protected set; } = 200;

		public override float Weight => 2f;
	}

}
