using System;
using System.Threading.Tasks;

namespace Assets.scripts
{
	public enum AngryBirdsGameObjects
	{
		Pig, BuildMaterial, Bird , Egg
	}
	public delegate void ObjectDieDelegate();
	public abstract class AngryBirdsGameObject
	{
		public virtual float Health { get; protected set; } = 1;
		public abstract float Armor { get; protected set; }
		public abstract short SpriteCoount { get; }
		public virtual float Mass => 1;
		public event ObjectDieDelegate ObjectDie = null;
		public virtual void GetDamage(float damage)
		{
			if (damage >= 1)
			{
				Armor -= 0.8f * damage;
				Health -= 0.2f * damage;
				if (Armor < 0)
				{
					Health -= Math.Abs(Armor);
					Armor = 0;
				}
				if (Health < 0 && this is Pig)
				{
					ObjectDie.Invoke();
				}				
				else if (Health < 0) 
				{
					Health = 0;
				}
			}
		}
		public virtual void InvokeDiedEvent()
		{
			if(Health == 0)
				ObjectDie.Invoke();			
		}

		public static AngryBirdsGameObjects GetTypesOfGameObject(string tag) => tag switch
		{
			"Bird" => AngryBirdsGameObjects.Bird,
			"Pig" => AngryBirdsGameObjects.Pig,
			"Build Material" => AngryBirdsGameObjects.BuildMaterial,
			"Egg" => AngryBirdsGameObjects.Egg,
			_ => throw new NotImplementedException()
		};
		public static AngryBirdsGameObject GetGameObjectType(AngryBirdsGameObjects type) => type switch
		{
			AngryBirdsGameObjects.Pig => new Pig(),
			AngryBirdsGameObjects.Bird => new Bird(),
			AngryBirdsGameObjects.BuildMaterial => new BuildMaterial(),
			AngryBirdsGameObjects.Egg => new Egg(),
			_ => throw new NotImplementedException()
		};
	}
}
