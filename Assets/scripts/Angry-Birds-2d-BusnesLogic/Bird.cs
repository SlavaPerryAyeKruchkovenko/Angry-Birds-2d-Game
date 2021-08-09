
using Assets.scripts.Angry_Birds_2d_BusnesLogic;
using System;
using System.Threading;
using UnityEngine;

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

        public event BirdStartFly StartFly = null;

        public event TakeAim TakeAim = null;

        public event ResetBird ResetBird = null;
        public override short SpriteCoount => 2;
        public readonly CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
		public override void GetDamage(float damage)
		{
            cancelTokenSource.Cancel();
            Health -= damage;
            if (Health < 0)
                Health = 0;
		}
		public void InvokeFlyEvent(Vector3 range)
		{
            if(ReadyFly!= null)
                ReadyFly.Invoke(range);
            if (StartFly != null)
                StartFly.Invoke();
            IsFly = true;
        }
        public void InvokeTakeAimEvent(Vector3 range)
		{
            if (TakeAim != null)
                TakeAim.Invoke(range);
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
                case (Birds.Black): if (material == BuildMaterials.Stone) { return 2; } break;
                case (Birds.Yellow): if (material == BuildMaterials.Wood) { return 3; } break;
                case (Birds.Blue): if (material == BuildMaterials.Ice) { return 4; } break;
                case (Birds.White): if (material == BuildMaterials.Stone) { return 5; } break;
                case (Birds.BigRed): if (material == BuildMaterials.Wood) { return 2; } break;
                case (Birds.Green): if (material == BuildMaterials.Wood) { return 2; } break;
            }
            return 1;
        }
    }
    public class BirdWithPower : Bird, IBird
	{
        public BirdWithPower(Power _power)
        {
            power = _power;
            this.ObjectDie += cancelTokenSource.Cancel;
        }
        private readonly Power power;
        protected bool canUsePower = true;

        public virtual TypeUsingAbility Ability => TypeUsingAbility.Click;
		public void UsePower()
        {
            CancellationToken token = cancelTokenSource.Token;
            if (canUsePower && !token.IsCancellationRequested)
			{
                power(cancelTokenSource);
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
        public BlueBird(Power _power, bool isClone):base(_power)
		{
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
		public override TypeUsingAbility Ability => TypeUsingAbility.Universal;
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
