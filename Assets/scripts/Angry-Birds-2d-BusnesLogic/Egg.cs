using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts
{
	class Egg : AngryBirdsGameObject
	{
		public override float Armor { get; protected set; } = 0;

		public override short SpriteCoount => 0;
		public override float Mass => 10;
	}
}
