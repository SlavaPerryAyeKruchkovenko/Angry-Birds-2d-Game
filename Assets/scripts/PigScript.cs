
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class PigScript : MonoBehaviour
{
    public int Health;
    public int Armor;
    public List<Sprite> Default;
    public List<Sprite> Damaged;
    public List<Sprite> SoDamaged;

	private async void Start()
	{
        while (true)
        {
            await Task.Delay(new System.Random().Next(1000, 10000));
            if (Health <= 0) { break; }
            ChangeCondition();
        }
	}
    void Update()
    {
        if (Health <= 0 && gameObject.GetComponent<Rigidbody2D>().velocity.sqrMagnitude == 0)
            Destroy(this.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ChangeCondition();
        Health -= Convert.ToInt32(collision.relativeVelocity.sqrMagnitude * collision.gameObject.GetComponent<Rigidbody2D>().mass);
    }
    private void ChangeCondition()
	{        
        if (Health > 66) { ChangeSprite(Default); }    
        else if (Health > 33 && Health <= 66) { ChangeSprite(Damaged); }
        else if (Health <= 33) { ChangeSprite(SoDamaged); }
    }
    private void ChangeSprite(List<Sprite> sprites) => gameObject.GetComponent<SpriteRenderer>().sprite = sprites[new System.Random().Next(0, sprites.Count)];
}
