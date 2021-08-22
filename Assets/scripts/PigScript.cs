
using Assets.scripts;
using Assets.scripts.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


internal class PigScript : MonoBehaviour
{
	[SerializeField]
	private List<Sprite> Default;
	[SerializeField]
	private List<Sprite> Damaged;
	[SerializeField]
	private List<Sprite> SoDamaged;
	private Pig pig;
	private async void Awake()
	{
		var game = GetComponent<GameObjectScript>();
		game.Awake();
		pig = game.ABGameObj as Pig;
		pig.ObjectDie += IsWin;
		pig.ObjectDie += () => Destroy(gameObject);
		while (true)
		{
			if (pig.Health <= 0) { break; }
			await Task.Delay(new System.Random().Next(1000, 10000));
			if (this == null)
				break;
			ChangeCondition(pig.Health);
		}
	}
	private void ChangeCondition(float health)
	{
		if (gameObject)
		{
			if (health > 66) { ChangeSprite(Default); }
			else if (health > 33 && health <= 66) { ChangeSprite(Damaged); }
			else if (health <= 33) { ChangeSprite(SoDamaged); }
		}
	}
	private void ChangeSprite(List<Sprite> sprites)
	{
		GetComponent<SpriteRenderer>().sprite = sprites[new System.Random().Next(0, sprites.Count)];
	}
	private void IsWin()
	{
		var pigs = GameObject.FindGameObjectsWithTag("Pig");
		if (pigs.Length > 1)
			return;
		GameViewModel.LevelComplete();		
	}
}
