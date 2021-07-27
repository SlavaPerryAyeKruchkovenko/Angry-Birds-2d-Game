/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

namespace Assets.scripts
{

    public enum Birds
    {
        Red, Blue, Yellow, Black, Green, White, BigRed
    }

    public abstract class Bird
    {
        public abstract void Power();
        public static Bird GetType(Birds TypeOfBird)
        {
            return TypeOfBird switch
            {
                Birds.Red => new RedBird(),
                Birds.Blue => new BlueBird(),
                Birds.Yellow => new YellowBird(),
                Birds.Black => new BlackBird(),
                Birds.Green => new GreenBird(),
                Birds.White => new WhiteBird(),
                Birds.BigRed => new BigRedBird(),
                _ => throw new NotImplementedException()
            };
        }
    }
    public class RedBird : Bird
    {
        public override void Power() { }
    }
    public class BlueBird: Bird
    {
        public override void Power(GameObject @object)
        {
            @object.Instantiate(ExtraObject, new Vector3(gameObject.transform.position.x - 10, gameObject.transform.position.y - 10), gameObject.transform.rotation);
            @object.Instantiate(@objectExtraObject, new Vector3(gameObject.transform.position.x + 10, gameObject.transform.position.y - 10), gameObject.transform.rotation);
        }
    }
}
*/