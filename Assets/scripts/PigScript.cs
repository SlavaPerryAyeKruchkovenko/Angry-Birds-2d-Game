
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class PigScript : MonoBehaviour
{
    public List<Sprite> Default;
    public List<Sprite> Damaged;
    public List<Sprite> SoDamaged;

	private async void Start()
	{       
        while (true)
        {
            await Task.Delay(new System.Random().Next(1000, 10000));
            if (this.gameObject.GetComponent<GameObjectScript>().Type.Health <= 0) { break; }
            ChangeCondition(this.gameObject.GetComponent<GameObjectScript>().Type.Health);
        }
	}
    private void ChangeCondition(float health)
	{        
        if (health > 66) { ChangeSprite(Default); }    
        else if (health > 33 && health <= 66) { ChangeSprite(Damaged); }
        else if (health <= 33) { ChangeSprite(SoDamaged); }
    }
    private void ChangeSprite(List<Sprite> sprites) => gameObject.GetComponent<SpriteRenderer>().sprite = sprites[new System.Random().Next(0, sprites.Count)];
}
