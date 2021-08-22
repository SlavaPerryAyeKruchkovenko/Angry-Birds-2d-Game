using Assets.scripts;
using Assets.scripts.Angry_Birds_2d_BusnesLogic;
using Assets.scripts.Converters;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

internal class GameObjectScript : MonoBehaviour
{
	public const float g = 9.8f;
	public AngryBirdsGameObjects TypeOfGameObj;
	private float maxHealth;
	public Pigs PigType;
	public Birds BirdType;
	public BuildMaterials MaterialType;
	public AngryBirdsGameObject ABGameObj { get; private set; }
	public List<Sprite> ConditionalSprites;
	public void Awake()//Found Type of AngryBirds object
	{
		if (ABGameObj == null)
		{
			ABGameObj = AngryBirdsGameObject.GetGameObjectType(TypeOfGameObj);
			SetStartSettings();
			//ABGameObj.ObjectDie += () => gameObject.SetActive(false);
		}
	}
	public void SetStartSettings()
	{				
		switch (ABGameObj)
		{
			case Pig _:
				ABGameObj = Pig.GetPig(PigType);// Get pig type (armor pig, king pig...) 				
				break;
			case Bird _:
				ABGameObj = Bird.GetBird(BirdType, new Powers(gameObject));
				break;
			case BuildMaterial _:
				ABGameObj = BuildMaterial.GetBuildMaterial(MaterialType);
				ABGameObj.ObjectDie += () => Destroy(gameObject);
				break;
			case Egg _:
				ABGameObj.ObjectDie += () => Destroy(gameObject);
				break;
		}
		var rigidbody = gameObject.GetComponent<Rigidbody2D>();
		if (rigidbody)
			rigidbody.mass = ABGameObj.Mass;

		maxHealth = ABGameObj.Health;// const that compare it with now health    		
		ABGameObj.ObjectGetDamage += ChangeConditional;
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		var rigidbody = gameObject.GetComponent<Rigidbody2D>();
		if (rigidbody)
		{
			float damage = CountDamage(gameObject, collision);
			ABGameObj.GetDamage(damage);
			if(ABGameObj is Bird)
			{
				ChangeSpeed(rigidbody, collision, ABGameObj.Mass);
			}		
		}
	}
	private static void ChangeSpeed(Rigidbody2D rigidbody, Collision2D collision, float mass)
	{
		if (rigidbody.velocity.magnitude > 4)
		{
			var rigidbodyColision = collision.gameObject.GetComponent<Rigidbody2D>();
			Vector2 speed = new Vector2(rigidbody.velocity.x / 5, rigidbody.velocity.y / 5);
			rigidbody.velocity -= speed;
			
			if (rigidbodyColision)
			{
				rigidbodyColision.velocity += speed / mass;
			}
		}
	}
	private void Update()
	{
		var rigidbody = GetComponent<Rigidbody2D>();
		if (rigidbody && rigidbody.velocity.sqrMagnitude <= 0.005 && ABGameObj.Health == 0)
			ABGameObj.InvokeDiedEvent();
	}

	private void ChangeConditional()
	{
		for (float i = ABGameObj.SpriteCoount - 1; i >= 0; i--)//Condisional status defined by count sprites and health
		{
			if (ABGameObj.Health >= i / ABGameObj.SpriteCoount * maxHealth && ABGameObj.Health < (i + 1) / ABGameObj.SpriteCoount * maxHealth)
			{
				ChangeSprite(gameObject, ConditionalSprites[(int)(ABGameObj.SpriteCoount - 1 - i)]);
				return;
			}
		}
	}
	private static void ChangeSprite(GameObject gameObject, Sprite sprite) =>
		gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
	private static float CountDamage(GameObject gameObject, Collision2D collision)
	{
		float damage = collision.relativeVelocity.magnitude;
		GameObject damagedObj = collision.gameObject;
		var angryBirdsObj = gameObject.GetComponent<GameObjectScript>();
		var rigidbody = gameObject.GetComponent<Rigidbody2D>();
		var colisionRigidbody = damagedObj.GetComponent<Rigidbody2D>();

		if(rigidbody.velocity.magnitude >= 1)
		{
			damage *= rigidbody.velocity.magnitude;// second Newton's law
		}		
		if (colisionRigidbody)
		{
			damage *= colisionRigidbody.mass;
		}
		else//if object down on ground
		{
			damage *= rigidbody.mass * g;
		}
		var script = damagedObj.GetComponent<GameObjectScript>();
		if(script)
		{
			var ABGameObject = angryBirdsObj.ABGameObj;
			if (ABGameObject is Bird && script.ABGameObj is Pig)
			{
				damage *= Bird.CountDamegedPig(angryBirdsObj.BirdType);
			}
			if (angryBirdsObj is IBird && script.ABGameObj is BuildMaterial)
			{
				damage *= Bird.CountDamageAbility(angryBirdsObj.BirdType, script.MaterialType);// mul on ability damage
			}
		}		
		return Mathf.Abs(damage);
	}
}
