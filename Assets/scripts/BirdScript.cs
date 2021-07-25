using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using Assets.scripts;

public class BirdScript : MonoBehaviour
{
    public Birds TypeOfBird;
    public List<Sprite> FlyingSprites;
    public List<Sprite> PowerSprites;
    private int health;
    public bool WasThrow { get; private set; } = false;


    private Action StartPower { get; set; }
    private bool IsPowerActivated { get; set; } = false;

    void Start()
    {
        StartPower = GetPower();
    }

    void Update()
    {
        if(this.GetComponent<Rigidbody2D>())
		{
            WasThrow = true;
		}
        if (WasThrow && gameObject.GetComponent<Rigidbody2D>().velocity.sqrMagnitude < 0.005)
		{
            Destroy(this.gameObject);
        }           
    }

    public async void StartFlying()
    {
        foreach (var sprite in FlyingSprites)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
            await Task.Delay(700);
            if (IsPowerActivated) break;
        }
    }

    public async void ActivatePower()
    {
        foreach(var sprite in PowerSprites)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
            if (sprite == PowerSprites[PowerSprites.Count - 1]) await Task.Delay(700);
        }
        StartPower();
    }

    private void RedBirdPower() { }

    private void BlueBirdPower() 
    { 

    }

    private async void YellowBirdPower() 
    {
        var rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        while (health > 0) 
        {
            var startX = rigidbody2D.velocity.x;
            var startY = rigidbody2D.velocity.y;
            rigidbody2D.velocity = new Vector2(startX * (float)1.5, startY * (float)1.5);
            await Task.Delay(1000);
        }
        
    }

    private void BlackBirdPower() 
    {
        var collider = gameObject.GetComponent<CircleCollider2D>();
        var rigidbody = gameObject.GetComponent<Rigidbody2D>();
        rigidbody.mass = 100;
        collider.radius *= 3;
    }

    private void GreenBirdPower()
    {
        var angle = Mathf.Atan2(gameObject.transform.position.y, gameObject.transform.position.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void WhiteBirdPower()
    {

    }

    private void BigRedBirdPower()
    {

    }

    private Action GetPower()
    {
        return TypeOfBird switch
        {
            Birds.Red => RedBirdPower,
            Birds.Blue => BlueBirdPower,
            Birds.Yellow => YellowBirdPower,
            Birds.Black => BlackBirdPower,
            Birds.Green => GreenBirdPower,
            Birds.White => WhiteBirdPower,
            Birds.BigRed => BigRedBirdPower,
            _ => throw new NotImplementedException()
        };
    }
}
