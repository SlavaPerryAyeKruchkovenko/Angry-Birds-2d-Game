
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class pigScript : MonoBehaviour
{
    public int Health;
    public int Armor;
    public List<Sprite> DefaultPig;
    public List<Sprite> DamagedPig;
    public List<Sprite> SoDamagedPig;

    private int timer;
    private List<Sprite> nowSprites;
    private short imageNumNow;

	private void Start()
	{
        
	}
	// Update is called once per frame
	void Update()
    {
        timer++;
        if (timer >= Random.Range(1000, 10000))
        {
            timer = 0;
            Timer_Elased();
        }
        if(ChangeCondition())
		{
            timer = 0;
            imageNumNow = 0;
            ChangeSprite();
        }
    }
	private bool ChangeCondition()
	{        
        if (this.Health > 66 && nowSprites != this.DefaultPig)
        {
            nowSprites = this.DefaultPig;
            return true;
        }    
        else if (this.Health <= 66 && nowSprites == this.DefaultPig)
        {
            nowSprites = this.DamagedPig;
            return true;
        }
        else if (this.Health <= 33 && nowSprites == this.DamagedPig)
        {
            nowSprites = this.SoDamagedPig;
            return true;
        }
        else
		{
            return false;
		}
    }
	private void Timer_Elased()
	{
        if (imageNumNow < nowSprites.Count - 1)
		{
            imageNumNow++;
        }           
        else
		{
            imageNumNow = 0;
        }
        ChangeSprite();
    }
    private void ChangeSprite() =>
        this.gameObject.GetComponent<SpriteRenderer>().sprite = nowSprites[imageNumNow];
}
