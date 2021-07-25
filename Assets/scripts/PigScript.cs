
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class PigScript : MonoBehaviour
{
    public float Health;
    public float Armor;
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
        if (Health <= 0 && gameObject.GetComponent<Rigidbody2D>().velocity.sqrMagnitude < 0.005)
            Destroy(this.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        float damage = collision.relativeVelocity.sqrMagnitude * collision.gameObject.GetComponent<Rigidbody2D>().mass;
        if(damage > 1)
		{
            Armor -= 0.666f * damage;
            Health -= 0.333f * damage;
            if (Armor < 0)
            {
                Health -= Math.Abs(Armor);
                Armor = 0;
            }
            ChangeCondition();
        }
        
    }
    private void ChangeCondition()
	{        
        if (Health > 66) { ChangeSprite(Default); }    
        else if (Health > 33 && Health <= 66) { ChangeSprite(Damaged); }
        else if (Health <= 33) { ChangeSprite(SoDamaged); }
    }
    private void ChangeSprite(List<Sprite> sprites) => gameObject.GetComponent<SpriteRenderer>().sprite = sprites[new System.Random().Next(0, sprites.Count)];
}
