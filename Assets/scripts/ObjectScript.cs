
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    public int Health;
    public int Armor;
    public List<Sprite> DefaultPig;
    public List<Sprite> DamagedPig;
    public List<Sprite> SoDamagedPig;

	private async void Start()
	{
        while (true)
        {
            await Task.Delay(new System.Random().Next(1000, 10000));
            ChangeCondition();
        }
	}
	// Update is called once per frame
	void Update()
    {

    }
	private void ChangeCondition()
	{        
        if (Health > 66) { ChangeSprite(DefaultPig); }    
        else if (Health <= 66) { ChangeSprite(DamagedPig); }
        else if (Health <= 33) { ChangeSprite(SoDamagedPig); }
    }
    private void ChangeSprite(List<Sprite> sprites) => gameObject.GetComponent<SpriteRenderer>().sprite = sprites[new System.Random().Next(0, sprites.Count)];
}
