using Assets.scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectScript : MonoBehaviour
{
    public TypesOfGameObject TypeOfGameObj;
    private float maxHealth;
    public Pigs PigType;
    public Birds BirdType;
    public BuildMaterials MaterialType;
    public TypeOfGameObject GameObj { get; private set; }
    public List<Sprite> ConditionalSprites;
	public void Start()
	{
        GameObj = TypeOfGameObject.GetGameObjectType(TypeOfGameObj);
        if (GameObj is Pig)
            GameObj = Pig.GetPig(PigType);

        else if (GameObj is Bird)
            GameObj = Bird.GetBird(BirdType);

        else if (GameObj is BuildMaterial)
            GameObj = BuildMaterial.GetBuildMaterial(MaterialType);

        if(this.gameObject.GetComponent<Rigidbody2D>())
            this.gameObject.GetComponent<Rigidbody2D>().mass = GameObj.Weight;
        maxHealth = GameObj.Health;
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
        if (this.gameObject.GetComponent<Rigidbody2D>())
		{
            damage *= this.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;
        }
        else
		{
            return;
		}
        damage = Mathf.Abs(damage);
        GameObj.GetDamage(damage);
        ChangeConditional();
    }
    // Update is called once per frame
    void Update()
    {
        if (GameObj.Health <= 0 && gameObject.GetComponent<Rigidbody2D>().velocity.sqrMagnitude < 0.005)
            Destroy(this.gameObject);

    }
    private void ChangeConditional()
	{

		for (float i = GameObj.SpriteCoount-1; i >= 0; i--)
		{
            if (GameObj.Health >= i / GameObj.SpriteCoount * maxHealth && GameObj.Health < (i + 1) / GameObj.SpriteCoount * maxHealth) 
			{
                ChangeSprite(this.gameObject, ConditionalSprites[(int)(GameObj.SpriteCoount - 1 - i)]);
            }
		}
    }
    private static void ChangeSprite(GameObject gameObject, Sprite sprite) =>
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
	
}
