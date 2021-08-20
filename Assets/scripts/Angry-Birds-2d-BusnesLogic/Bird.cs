
using Assets.scripts.Angry_Birds_2d_BusnesLogic;
using System;
using System.Numerics;
using System.Threading;

namespace Assets.scripts
{  
    public enum Birds
    {
        Red, Blue, Yellow, Black, Green, White, BigRed, BlueClone
    }
    public delegate void BirdReadyFly(Vector3 range);
    public delegate void BirdStartFly();
    public delegate void TakeAim(Vector3 range);
    public delegate void ResetBird();

    public delegate void Power(CancellationTokenSource cancelTokenSource);
    public class Bird : AngryBirdsGameObject
    {
        public bool IsFly { get; private set; } = false;
        public override float Health { get; protected set; } = 1;
        public override float Armor { get; protected set; } = 0;

        public event BirdReadyFly ReadyFly = null;

        public event Action StartFly = null;

        public event Action<Vector3> ChangeBird = null;

        public event Action ResetBird = null;
        public event Action<Vector3> TakeAim = null;
        public override short SpriteCoount => 0;
        public readonly CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
		public override void GetDamage(float damage)
		{
            cancelTokenSource.Cancel();
            Health -= damage;
            if (Health < 0)
                Health = 0;
            InvokeDamageEvent();
        }
		public void InvokeFlyEvent(Vector3 range)
		{
            IsFly = true;
            if (ReadyFly!= null)
                ReadyFly.Invoke(range);
            if (StartFly != null)
                StartFly.Invoke();          
        }
        public void InvokeChangeBirdEvent(Vector3 range)
		{
            if (ChangeBird != null)
                ChangeBird.Invoke(range);          
		}
        public void InvokeTakeAim(Vector3 value)
		{
            if (TakeAim != null)
                TakeAim.Invoke(value);
        }
        public void InvokeResetEvent()
		{
            ResetBird.Invoke();
		}
		public override void InvokeDiedEvent()
		{
            IsFly = false;
			base.InvokeDiedEvent();
		}
		public static Bird GetBird(Birds bird, IPowers powersRealeasetion)
        {
            return bird switch
            {
                Birds.Red => new RedBird(),
                Birds.Blue => new BlueBird(powersRealeasetion.Clone , false),
                Birds.Yellow => new YellowBird(powersRealeasetion.SpeedUp),
                Birds.Black => new BlackBird(powersRealeasetion.Explode),
                Birds.Green => new GreenBird(powersRealeasetion.UTurn),
                Birds.White => new WhiteBird(powersRealeasetion.DropEgg),
                Birds.BigRed => new BigRedBird(),
                Birds.BlueClone => new BlueBird(powersRealeasetion.Clone,true),
                _ => throw new Exception("Bird not found"),
            };
        }
        public static float CountDamageAbility(Birds bird, BuildMaterials material)// Calculate Damage for bird with ability
        {
			switch (bird)
			{
				case Birds.Black: if (material == BuildMaterials.Stone) { return 10; } break;
				case Birds.Yellow: if (material == BuildMaterials.Wood) { return 10; } break;
				case Birds.Blue: if (material == BuildMaterials.Ice) { return 10; } break;
				case Birds.White: if (material == BuildMaterials.Stone) { return 5; } break;
				case Birds.BigRed: return 15;
				case Birds.Green: if (material == BuildMaterials.Wood) { return 7; } break;
				case Birds.BlueClone: if (material == BuildMaterials.Ice) { return 10; } break;
				case Birds.Red: return 5;
			}
            return 2;
        }
		public static float CountDamegedPig(Birds bird)
		{
			return bird switch
			{
				Birds.Black => 10,
				Birds.Yellow => 5,
				Birds.Blue => 3,
				Birds.White => 7,
				Birds.BigRed => 15,
				Birds.Green => 7,
				Birds.Red => 5,
				Birds.BlueClone => 3,
				_ => 1,
			};
		}
	}
	public class BirdWithPower : Bird, IBird
	{
        public BirdWithPower(Power _power)
        {
            Ability += _power;
            ObjectDie += cancelTokenSource.Cancel;
        }
        public event Power Ability;
        protected bool canUsePower = true;

        public virtual TypeUsingAbility AbilityType => TypeUsingAbility.Click;
		public void UsePower()
        {
            CancellationToken token = cancelTokenSource.Token;
            if (canUsePower && !token.IsCancellationRequested)
			{
                Ability.Invoke(cancelTokenSource);
                canUsePower = false;
            }                
        }
    }
    public class RedBird : Bird
    {
		public override float Mass => 1;
	}
    public class BlueBird: BirdWithPower
    {
        public readonly bool IsClone;
        public BlueBird(Power _power, bool isClone):base(_power)
		{
            IsClone = isClone;
            canUsePower = !isClone;
            if(!canUsePower)
			{
                cancelTokenSource.Cancel();
			}
		}
        public override float Mass => 0.5f;
	}
    public class YellowBird : BirdWithPower
    {
        public YellowBird(Power _power) : base(_power)
        {

        }
        public override float Mass => 1;
    }
    public class BigRedBird : Bird
    {
        public override float Mass => 2;
    }
    public class BlackBird : BirdWithPower
    {
        public BlackBird(Power _power) : base(_power)
        {

        }
		public override TypeUsingAbility AbilityType => TypeUsingAbility.Universal;
		public override float Mass => 1.2f;
    }
    public class WhiteBird : BirdWithPower
    {
        public WhiteBird(Power _power) : base(_power)
        {

        }
        public override float Mass => 1.5f;
    }
    public class GreenBird : BirdWithPower
    {
        public GreenBird(Power _power) : base(_power)
        {

        }
        public override float Mass => 1;
    }

}
