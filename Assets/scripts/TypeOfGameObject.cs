using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.scripts
{
	public enum TypesOfGameObject
	{
		Pig, BuildMaterial, Bird
	}
	public abstract class TypeOfGameObject
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
		public static TypesOfGameObject GetTypesOfGameObject(GameObject gameObject) => gameObject.tag switch
		{
			"Bird" => TypesOfGameObject.Bird,
			"Pig" => TypesOfGameObject.Pig,
			"Build Material" => TypesOfGameObject.BuildMaterial,
			_ => throw new NotImplementedException()
		};
		public static TypeOfGameObject GetGameObjectType(TypesOfGameObject type) => type switch
		{
			TypesOfGameObject.Pig => new Pig(),
			TypesOfGameObject.Bird => new Bird(),
			TypesOfGameObject.BuildMaterial => new BuildMaterial(),
			_ => throw new NotImplementedException()
		};
	}
}
