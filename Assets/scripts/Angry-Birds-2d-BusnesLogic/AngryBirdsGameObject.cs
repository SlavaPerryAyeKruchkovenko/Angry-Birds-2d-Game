using System;

namespace Assets.scripts
{
	public enum AngryBirdsGameObjects
	{
		Pig, BuildMaterial, Bird
	}
	public abstract class AngryBirdsGameObject
	{		
		public abstract float Health { get; protected set; }
		public abstract float Armor { get; protected set; }
		public abstract short SpriteCoount { get; }
		public virtual float Weight => 1;
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
				if (Health < 0) 
				{
					Health = 0;
				}
			}
		}
		public static AngryBirdsGameObjects GetTypesOfGameObject(string tag) => tag switch
		{
			"Bird" => AngryBirdsGameObjects.Bird,
			"Pig" => AngryBirdsGameObjects.Pig,
			"Build Material" => AngryBirdsGameObjects.BuildMaterial,
			_ => throw new NotImplementedException()
		};
		public static AngryBirdsGameObject GetGameObjectType(AngryBirdsGameObjects type) => type switch
		{
			AngryBirdsGameObjects.Pig => new Pig(),
			AngryBirdsGameObjects.Bird => new Bird(),
			AngryBirdsGameObjects.BuildMaterial => new BuildMaterial(),
			_ => throw new NotImplementedException()
		};
	}
}
