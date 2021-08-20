using System;

namespace Assets.scripts
{
	public enum BuildMaterials
	{
		Ice, Wood, Stone
	}
	public class BuildMaterial :AngryBirdsGameObject
	{
		public override short SpriteCoount => 4;
		public override float Health { get ; protected set; }
		public override float Armor { get ; protected set; }

		public static BuildMaterial GetBuildMaterial(BuildMaterials material) => material switch
		{
			BuildMaterials.Ice => new IceBuildMaterial(),
			BuildMaterials.Wood => new WoodBildMaterial(),
			BuildMaterials.Stone => new StoneBildMaterial(),
			_ => throw new NotImplementedException()
		};
		
	}
	public class IceBuildMaterial : BuildMaterial
	{
		public override float Health { get; protected set; } = 50;

		public override float Armor { get; protected set; } = 0;

		public override float Mass => 0.2f;
	}
	public class WoodBildMaterial : BuildMaterial
	{
		public override float Health { get; protected set; } = 100;

		public override float Armor { get; protected set; } = 0;

		public override float Mass => 0.5f;
	}
	public class StoneBildMaterial : BuildMaterial
	{
		public override float Health { get; protected set; } = 100;

		public override float Armor { get; protected set; } = 100;

		public override float Mass => 2f;
	}

}
