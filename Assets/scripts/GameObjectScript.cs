using Assets.scripts;
using System.Collections;
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
	private void OnDestroy()
	{
        this.ABGameObj = null;
	}
	public void Awake()//Found Type of AngryBird object
    {
        if (ABGameObj == null)
        {
            ABGameObj = AngryBirdsGameObject.GetGameObjectType(TypeOfGameObj);
            if (ABGameObj is Pig)
                ABGameObj = Pig.GetPig(PigType);// Get pig type (armor pig, king pig...)

            else if (ABGameObj is Bird)
                ABGameObj = Bird.GetBird(BirdType);

            else if (ABGameObj is BuildMaterial)
                ABGameObj = BuildMaterial.GetBuildMaterial(MaterialType);

            if (this.gameObject.GetComponent<Rigidbody2D>())
                this.gameObject.GetComponent<Rigidbody2D>().mass = ABGameObj.Mass;
            maxHealth = ABGameObj.Health;// const that compare it with now health    
            ABGameObj.ObjectDie += () => Destroy(this.gameObject);
        }         
    }
	// Start is called before the first frame update
	private void OnCollisionEnter2D(Collision2D collision)
	{
        float damage = collision.relativeVelocity.magnitude;
        if (collision.gameObject.GetComponent<Rigidbody2D>())
		{
            damage *= collision.gameObject.GetComponent<Rigidbody2D>().mass;
        }
        else//if object down on ground
		{
            damage *= 5;
		}
        if (this.gameObject.GetComponent<Rigidbody2D>())// second Newton's law
        {
            damage *= this.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;
        }
        else// if object hasn't rigid body it isn't 'Game' object
		{
            return;
		}
        damage = Mathf.Abs(damage);
        ABGameObj.GetDamage(damage);
        ChangeConditional();
    }
    private void Update()
	{
        if (gameObject.GetComponent<Rigidbody2D>() && gameObject.GetComponent<Rigidbody2D>().velocity.sqrMagnitude <= 0.005)
		{            
            ABGameObj.InvokeDiedEvent();
        }
            
    }
    private void ChangeConditional()
	{

		for (float i = ABGameObj.SpriteCoount-1; i >= 0; i--)//Condisional status defined by count sprites and health
        {
            if (ABGameObj.Health >= i / ABGameObj.SpriteCoount * maxHealth && ABGameObj.Health < (i + 1) / ABGameObj.SpriteCoount * maxHealth) 
			{
                ChangeSprite(this.gameObject, ConditionalSprites[(int)(ABGameObj.SpriteCoount - 1 - i)]);
            }
		}
    }
    private static void ChangeSprite(GameObject gameObject, Sprite sprite) =>
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
	
}
