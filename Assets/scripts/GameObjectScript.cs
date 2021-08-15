using Assets.scripts;
using Assets.scripts.Angry_Birds_2d_BusnesLogic;
using Assets.scripts.Converters;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectScript : MonoBehaviour
{
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
			SetStartSettings();
		}
	}
	public void SetStartSettings()
	{		
		ABGameObj = AngryBirdsGameObject.GetGameObjectType(TypeOfGameObj);
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
				break;
		}
		var rigidbody = gameObject.GetComponent<Rigidbody2D>();
		if (rigidbody)
			rigidbody.mass = ABGameObj.Mass;

		maxHealth = ABGameObj.Health;// const that compare it with now health    
		ABGameObj.ObjectDie += () => Destroy(gameObject);
		ABGameObj.ObjectGetDamage += ChangeConditional;
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		var rigidbody = gameObject.GetComponent<Rigidbody2D>();
		if (rigidbody)
		{
			float damage = CountDamage(gameObject, collision);
			ABGameObj.GetDamage(damage);			
			ChangeSpeed(rigidbody, collision, ABGameObj.Mass);
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
		if (ABGameObj is BirdWithPower ibird && (ibird.AbilityType == TypeUsingAbility.TouchObject || ibird.AbilityType == TypeUsingAbility.Universal))
			return;
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

		damage *= rigidbody.velocity.magnitude;// second Newton's law
		if (colisionRigidbody)
		{
			damage *= colisionRigidbody.mass;
		}
		else//if object down on ground
		{
			damage *= 5;
		}
		var script = damagedObj.GetComponent<GameObjectScript>();
		if(script)
		{
			var ABGameObject = angryBirdsObj.ABGameObj;
			if (ABGameObject is Bird && script.ABGameObj is Pig)
			{
				damage *= 10;
			}
			if (angryBirdsObj is IBird && script.CompareTag("Build Material"))
			{
				damage *= Bird.CountDamageAbility(angryBirdsObj.BirdType, script.MaterialType);// mul on ability damage
			}
		}		
		return Mathf.Abs(damage);
	}

}
