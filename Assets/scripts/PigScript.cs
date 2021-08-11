
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class PigScript : MonoBehaviour
{
    public List<Sprite> Default;
    public List<Sprite> Damaged;
    public List<Sprite> SoDamaged;
    public Animator Animator;

	private async void Start()
	{
        while (true)
        {
            if (this.gameObject.GetComponent<GameObjectScript>().ABGameObj.Health <= 0) { break; }
            await Task.Delay(new System.Random().Next(1000, 10000));
            if (this == null)
                break;
            ChangeCondition(this.gameObject.GetComponent<GameObjectScript>().ABGameObj.Health);
        }
	}
    private void ChangeCondition(float health)
	{      
        if(this.gameObject)
		{
            if (health > 66) { ChangeSprite(Default); }
            else if (health > 33 && health <= 66) { ChangeSprite(Damaged); }
            else if (health <= 33) { ChangeSprite(SoDamaged); }
        }  
    }
    private void ChangeSprite(List<Sprite> sprites) => gameObject.GetComponent<SpriteRenderer>().sprite = sprites[new System.Random().Next(0, sprites.Count)];
}
