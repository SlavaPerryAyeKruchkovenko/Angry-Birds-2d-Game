using Assets.scripts;
using Assets.scripts.Angry_Birds_2d_BusnesLogic;
using Assets.scripts.Converters;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        if (ABGameObj is Pig)
            ABGameObj = Pig.GetPig(PigType);// Get pig type (armor pig, king pig...)

        else if (ABGameObj is Bird)
            ABGameObj = Bird.GetBird(BirdType, new Powers(this.gameObject));

        else if (ABGameObj is BuildMaterial)
            ABGameObj = BuildMaterial.GetBuildMaterial(MaterialType);

        if (this.gameObject.GetComponent<Rigidbody2D>())
            this.gameObject.GetComponent<Rigidbody2D>().mass = ABGameObj.Mass;

        maxHealth = ABGameObj.Health;// const that compare it with now health    
        ABGameObj.ObjectDie += () => Destroy(this.gameObject);
    }
	private void OnCollisionEnter2D(Collision2D collision)
	{
        if (gameObject.GetComponent<Rigidbody2D>())
        {
            var rigidbody = gameObject.GetComponent<Rigidbody2D>();

            float damage = CountDamage(this.gameObject, collision);
            ABGameObj.GetDamage(damage);
            ChangeConditional();
            if (rigidbody.velocity.magnitude > 4)
            {
                Vector2 speed = new Vector2(rigidbody.velocity.x / 5, rigidbody.velocity.y / 5);
                rigidbody.velocity -= speed;
                if (collision.gameObject.GetComponent<Rigidbody2D>())
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity += speed / ABGameObj.Mass;
                }
            }
        }          
    }
    private void Update()
	{
        if(ABGameObj.Health == 0)
		{
            if (gameObject.GetComponent<Rigidbody2D>() && gameObject.GetComponent<Rigidbody2D>().velocity.sqrMagnitude <= 0.005)
            {
                ABGameObj.InvokeDiedEvent();
            }
        }                
    }
	
	private void ChangeConditional()
	{
		for (float i = ABGameObj.SpriteCoount-1; i >= 0; i--)//Condisional status defined by count sprites and health
        {
            if (ABGameObj.Health >= i / ABGameObj.SpriteCoount * maxHealth && ABGameObj.Health < (i + 1) / ABGameObj.SpriteCoount * maxHealth) 
			{
                ChangeSprite(this.gameObject, ConditionalSprites[(int)(ABGameObj.SpriteCoount - 1 - i)]);
                return;
            }
		}
    }
    private static void ChangeSprite(GameObject gameObject, Sprite sprite) =>
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    private static float CountDamage(GameObject gameObject, Collision2D collision)
	{
        float damage = collision.relativeVelocity.magnitude;
        var damagedObj = collision.gameObject;
        var angryBirdsObj = gameObject.GetComponent<GameObjectScript>();

        damage *= gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;

        if (collision.gameObject.GetComponent<Rigidbody2D>())
        {
            damage *= collision.gameObject.GetComponent<Rigidbody2D>().mass;
        }
        else//if object down on ground
        {
            damage *= 5;
        }
        if (gameObject.GetComponent<Rigidbody2D>())// second Newton's law
        {
            damage *= gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;
        }
        if(damagedObj.GetComponent<GameObjectScript>())// mul on ability damage
		{
            var script = damagedObj.GetComponent<GameObjectScript>();
            if (angryBirdsObj.ABGameObj is IBird && script.CompareTag("Build Material"))
			{
                damage *= Bird.CountDamageAbility(angryBirdsObj.BirdType, script.MaterialType);
			}
        }       
        return Mathf.Abs(damage);        
    }	
    
}
