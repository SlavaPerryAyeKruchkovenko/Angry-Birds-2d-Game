﻿
using Assets.scripts.Angry_Birds_2d_BusnesLogic;
using System;
using System.Threading;

namespace Assets.scripts
{
    public enum Birds
    {
        Red, Blue, Yellow, Black, Green, White, BigRed, BlueClone
    }
    public delegate void Power(CancellationTokenSource cancelTokenSource);
    public class Bird : AngryBirdsGameObject
    {
        public override float Health { get; protected set; } = 1;
        public override float Armor { get; protected set; } = 0;
        public override short SpriteCoount => 2;
        public readonly CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        public static Bird GetBird(Birds bird, IPowers powersRealeasetion)
        {
            return bird switch
            {
                Birds.Red => new RedBird(),
                Birds.Blue => new BlueBird(powersRealeasetion.Clone , true),
                Birds.Yellow => new YellowBird(powersRealeasetion.SpeedUp),
                Birds.Black => new BlackBird(powersRealeasetion.Explode),
                Birds.Green => new GreenBird(powersRealeasetion.UTurn),
                Birds.White => new WhiteBird(powersRealeasetion.DropEgg),
                Birds.BigRed => new BigRedBird(),
                Birds.BlueClone => new BlueBird(powersRealeasetion.Clone,false),
                _ => throw new Exception("Bird not found"),
            };
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
		public BlueBird(Power _power, bool _canUsePower):base(_power)
		{
            canUsePower = _canUsePower;
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
		public override TypeUsingAbility Ability => TypeUsingAbility.TouchObject;
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
