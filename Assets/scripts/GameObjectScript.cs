using Assets.scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectScript : MonoBehaviour
{
    private TypesOfGameObject TypeOfGameObj;
    private float health;
    public Pigs PigType;
    public Birds BirdType;
    public BuildMaterials MaterialType;
    public TypeOfGameObject Type { get; private set; }
    public List<Sprite> ConditionalSprites;
	private void Start()
	{
        TypeOfGameObj = TypeOfGameObject.GetTypesOfGameObject(this.gameObject);
        Type = TypeOfGameObject.GetGameObjectType(TypeOfGameObj);
        if (Type is Pig)
        {
            Type = Pig.GetPig(PigType);
        }
        else if (Type is Bird)
        {
            Type = Bird.GetBird(BirdType);
        }
        else if (Type is BuildMaterial)
        {
            Type = BuildMaterial.GetBuildMaterial(MaterialType);
            BuildMaterial material = Type as BuildMaterial;
            this.gameObject.GetComponent<Rigidbody2D>().mass *= material.Weight;
        }
        health = Type.Health;
        if (Type.SpriteCoount != ConditionalSprites.Count)
		{
            Debug.LogError("Не хватает спрайтов");
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
        if (this.gameObject.GetComponent<Rigidbody2D>())
		{
            damage *= this.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;
        }
        damage = Mathf.Abs(damage);
        Type.GetDamage(damage);
        ChangeConditional();
    }
    // Update is called once per frame
    void Update()
    {
        if (Type.Health <= 0 && gameObject.GetComponent<Rigidbody2D>().velocity.sqrMagnitude < 0.005)
            Destroy(this.gameObject);

    }
    private void ChangeConditional()
	{

		for (float i = Type.SpriteCoount-1; i >= 0; i--)
		{
            if (Type.Health >= i / Type.SpriteCoount * health && Type.Health < (i + 1) / Type.SpriteCoount * health) 
			{
                ChangeSprite(this.gameObject, ConditionalSprites[(int)(Type.SpriteCoount - 1 - i)]);
            }
		}
    }
    private static void ChangeSprite(GameObject gameObject, Sprite sprite) =>
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
	
}
