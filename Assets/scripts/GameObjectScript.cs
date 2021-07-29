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
	public void Start()
	{
        ABGameObj = AngryBirdsGameObject.GetGameObjectType(TypeOfGameObj);
        if (ABGameObj is Pig)
            ABGameObj = Pig.GetPig(PigType);

        else if (ABGameObj is Bird)
            ABGameObj = Bird.GetBird(BirdType);

        else if (ABGameObj is BuildMaterial)
            ABGameObj = BuildMaterial.GetBuildMaterial(MaterialType);

        if(this.gameObject.GetComponent<Rigidbody2D>())
            this.gameObject.GetComponent<Rigidbody2D>().mass = ABGameObj.Weight;
        maxHealth = ABGameObj.Health;
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
        ABGameObj.GetDamage(damage);
        ChangeConditional();
    }
    // Update is called once per frame
    void Update()
    {
        if (ABGameObj.Health <= 0 && gameObject.GetComponent<Rigidbody2D>().velocity.sqrMagnitude <= 0.005)
            Destroy(this.gameObject);

    }
    private void ChangeConditional()
	{

		for (float i = ABGameObj.SpriteCoount-1; i >= 0; i--)
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
