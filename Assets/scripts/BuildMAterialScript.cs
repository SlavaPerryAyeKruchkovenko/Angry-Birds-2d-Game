using Assets.scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMaterialScript : MonoBehaviour
{
    public TypeOfBuildMaterial Material;
    private readonly BuildMaterial material;
    public List<Sprite> ConditionalSprites;
    public BuildMaterialScript()
	{
        material = BuildMaterial.GetBuildMaterial(Material);
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
	{
        float damage = collision.relativeVelocity.sqrMagnitude * collision.gameObject.GetComponent<Rigidbody2D>().mass;
        material.GetDamage(damage);
        ChangeConditional();
    }
    // Update is called once per frame
    void Update()
    {
        if (material.Health <= 0 && gameObject.GetComponent<Rigidbody2D>().velocity.sqrMagnitude < 0.005)
            Destroy(this.gameObject);

    }
    private void ChangeConditional()
	{
        if (material.Health >= 75)
        {
            ChangeSprite(this.gameObject, ConditionalSprites[0]);
        }
        else if(material.Health < 75 && material.Health >= 50)
		{
            ChangeSprite(this.gameObject, ConditionalSprites[1]);
        }
        else if(material.Health < 50 && material.Health >= 25)
		{
            ChangeSprite(this.gameObject, ConditionalSprites[2]);
        }
        else if (material.Health < 25 && material.Health >=0)
        {
            ChangeSprite(this.gameObject, ConditionalSprites[3]);
        }
    }
    private static void ChangeSprite(GameObject gameObject, Sprite sprite)
	{
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
	}
}
