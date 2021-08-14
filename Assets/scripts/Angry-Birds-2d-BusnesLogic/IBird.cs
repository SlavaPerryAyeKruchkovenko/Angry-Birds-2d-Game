using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.scripts.Angry_Birds_2d_BusnesLogic
{
	public enum TypeUsingAbility
	{
		Click, TouchObject, Universal
	}
	public interface IBird
	{
		void UsePower();
		TypeUsingAbility AbilityType { get; }
	}
	public interface IPowers
	{
		void Clone(CancellationTokenSource cancelTokenSource);
		void UTurn(CancellationTokenSource cancelTokenSource);
		void Explode(CancellationTokenSource cancelTokenSource);
		void DropEgg(CancellationTokenSource cancelTokenSource);
		void SpeedUp(CancellationTokenSource cancelTokenSource);
	}
	
}
