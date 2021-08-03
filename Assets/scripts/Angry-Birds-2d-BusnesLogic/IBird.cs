using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts.Angry_Birds_2d_BusnesLogic
{
	public enum TypeUsingAbility
	{
		Click, TouchObject
	}
	public interface IBird
	{
		void UsePower();
		TypeUsingAbility Ability { get; }
	}
	public interface IPowers
	{
		void Clone();
		void UTurn();
		void Explode();
		void DropEgg();
		void SpeedUp();
	}
	
}
