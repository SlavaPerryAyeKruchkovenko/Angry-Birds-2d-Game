using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Birds
{
	public enum Birds
	{
		Red, Blue, Yellow, Black, Green, White, BigRed
	}
	public abstract class Bird
	{
		public virtual int Health { get; protected set; } = 1;
		public Sprite DefaultImage { get; protected set; }
		public List<Sprite> AllBirdSprites;
		public abstract void ChangeConditional();
		public static Bird GetBird(Birds bird)
        {
            return bird switch
            {
                Birds.Red => new RedBird(),
                Birds.Blue => new BlueBird(true),
                Birds.Yellow => new YellowBird(true),
                Birds.Black => new BlackBird(true),
                Birds.Green => new GreenBird(true),
                Birds.White => new WhiteBird(true),
                Birds.BigRed => new BigRedBird(),
                _ => throw new Exception("Bird not found"),
            };
        }
	}
	public class RedBird : Bird
	{
		public override void ChangeConditional()
		{
			throw new NotImplementedException();
		}
	}
	public class BlueBird : Bird
	{
		public BlueBird(bool haveClone)
		{
			IsClone = haveClone;
		}
		public bool IsClone { get; private set; } = true;
		public BlueBird[] BlueBirdChildrens { get; private set; } = null;
		public void CloneBird()
		{
			if(IsClone)
			{
				BlueBirdChildrens = new BlueBird[] {new BlueBird(false), new BlueBird(false) , new BlueBird(false) };
				IsClone = false;
			}
			else
			{
				IsClone = false;
			}
		}
		public override void ChangeConditional()
		{
			throw new NotImplementedException();
		}
	}
	public class YellowBird : Bird
	{
		public YellowBird(bool canSpeedUp)
		{
			HaveNitro = canSpeedUp;
		}
		public int Speed { get; private set; }
		public bool HaveNitro { get; private set; }
		public void SpeedUp()
		{
			if(HaveNitro)
			{
				throw new Exception("Не реализовано");
				HaveNitro = false;
			}	
		}
		public override void ChangeConditional()
		{
			throw new NotImplementedException();
		}
	}
	public class BigRedBird : Bird
	{
		public override void ChangeConditional()
		{
			throw new NotImplementedException();
		}
	}
	public class BlackBird : Bird
	{
		public BlackBird(bool canExplode)
		{
			CanExplode = canExplode;
		}
		public bool CanExplode { get; private set; }

		public void Explode()
		{
			if(CanExplode)
			{
				throw new NotImplementedException();
			}
		}
		public override void ChangeConditional()
		{
			throw new NotImplementedException();
		}
	}
	public class WhiteBird : Bird
	{
		public WhiteBird(bool haveEgg)
		{
			HaveEgg = haveEgg;
		}
		public bool HaveEgg { get; private set; }

		public void LayAnEgg()
		{
			if (HaveEgg)
			{
				throw new NotImplementedException();
			}
		}
		public override void ChangeConditional()
		{
			throw new NotImplementedException();
		}
	}
	public class GreenBird : Bird
	{
		public GreenBird(bool canReversere)
		{
			CanReverseFly = canReversere;
		}
		public bool CanReverseFly { get; private set; }
		public override void ChangeConditional()
		{
			throw new NotImplementedException();
		}
	}

}
