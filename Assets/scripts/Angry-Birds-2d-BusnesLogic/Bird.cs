
using System;

namespace Assets.scripts
{
    public enum Birds
    {
        Red, Blue, Yellow, Black, Green, White, BigRed
    }
    public class Bird : AngryBirdsGameObject
    {
        public override float Health { get; protected set; } = 1;
        public override float Armor { get; protected set; } = 0;
        public override short SpriteCoount => 2;
        public static Bird GetBird(Birds bird)
        {
            return bird switch
            {
                Birds.Red => new RedBird(),
                Birds.Blue => new BlueBird(),
                Birds.Yellow => new YellowBird(),
                Birds.Black => new BlackBird(),
                Birds.Green => new GreenBird(),
                Birds.White => new WhiteBird(),
                Birds.BigRed => new BigRedBird(),
                _ => throw new Exception("Bird not found"),
            };
        }
    }
    public class RedBird : Bird
    {
        public override float Health { get; protected set; } = 1;
		public override float Weight => 1;
	}
    public class BlueBird : Bird
    {
        public override float Health { get; protected set; } = 1;
        public override float Weight => 0.5f;
    }
    public class YellowBird : Bird
    {
        public override float Health { get; protected set; } = 1;
        public override float Weight => 1;
    }
    public class BigRedBird : Bird
    {
        public override float Health { get; protected set; } = 1;
        public override float Weight => 2;
    }
    public class BlackBird : Bird
    {
        public override float Health { get; protected set; } = 1;
        public override float Weight => 1.2f;
    }
    public class WhiteBird : Bird
    {
        public override float Health { get; protected set; } = 1;
        public override float Weight => 1.5f;
    }
    public class GreenBird : Bird
    {
        public override float Health { get; protected set; } = 1;
        public override float Weight => 1;
    }

}
