using Assets.scripts;
using Assets.scripts.Angry_Birds_2d_BusnesLogic;
using Assets.scripts.Converters;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectScript : MonoBehaviour
{
    public AngryBirdsGameObjects TypeOfGameObj;
    public Pigs PigType;
    public Birds BirdType;
    public BuildMaterials MaterialType;
    public List<Sprite> ConditionalSprites;
    private float _maxHealth;

    public AngryBirdsGameObject ABGameObj { get; private set; }

    public void Awake()//Found Type of AngryBirds object
    {
        if (ABGameObj != null) return;
        SetStartSettings();
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

        var rigidBody = gameObject.GetComponent<Rigidbody2D>();
        if (rigidBody) rigidBody.mass = ABGameObj.Mass;

        _maxHealth = ABGameObj.Health;// const that compare it with now health    
        ABGameObj.ObjectDie += () => Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var rigidBody = gameObject.GetComponent<Rigidbody2D>();
        if (rigidBody)
        {
            float damage = CountDamage(gameObject, collision);
            ABGameObj.GetDamage(damage);
            ChangeConditional();
            if (rigidBody.velocity.magnitude > 4)
            {
                Vector2 speed = new Vector2(rigidBody.velocity.x / 5, rigidBody.velocity.y / 5);
                rigidBody.velocity -= speed;
                var rigidBodyCol = collision.gameObject.GetComponent<Rigidbody2D>();
                if (rigidBodyCol)
                {
                    rigidBodyCol.velocity += speed / ABGameObj.Mass;
                }
            }
        }
    }

    private void Update()
    {
        var rigidBody = gameObject.GetComponent<Rigidbody2D>();
        if (ABGameObj.Health == 0 &&
            rigidBody &&
            rigidBody.velocity.sqrMagnitude <= 0.005)
        {
            ABGameObj.InvokeDiedEvent();
        }
    }

    private void ChangeConditional()
    {
        for (float i = ABGameObj.SpriteCoount - 1; i >= 0; i--)//Condisional status defined by count sprites and health
        {
            if (ABGameObj.Health >= i / ABGameObj.SpriteCoount * _maxHealth && ABGameObj.Health < (i + 1) / ABGameObj.SpriteCoount * _maxHealth)
            {
                ChangeSprite(this.gameObject, ConditionalSprites[(int)(ABGameObj.SpriteCoount - 1 - i)]);
                return;
            }
        }
    }

    private static void ChangeSprite(GameObject gameObject, Sprite sprite)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    private static float CountDamage(GameObject gameObject, Collision2D collision)
    {
        float damage = collision.relativeVelocity.magnitude;
        var damagedObj = collision.gameObject;
        var angryBirdsObj = gameObject.GetComponent<GameObjectScript>();
        var rigidBody = gameObject.GetComponent<Rigidbody2D>();

        damage *= rigidBody.velocity.magnitude;

        CalculateCollisionDamage(collision, ref damage);

        if (rigidBody)// second Newton's law
        {
            damage *= rigidBody.velocity.magnitude;
        }
        if (damagedObj.GetComponent<GameObjectScript>())// mul on ability damage
        {
            var script = damagedObj.GetComponent<GameObjectScript>();
            if (angryBirdsObj.ABGameObj is IBird && script.CompareTag("Build Material"))
            {
                damage *= Bird.CountDamageAbility(angryBirdsObj.BirdType, script.MaterialType);
            }
        }
        return Mathf.Abs(damage);
    }

    private static void CalculateCollisionDamage(Collision2D collision, ref float damage)
    {
        var rigidBodyCol = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rigidBodyCol) { damage *= rigidBodyCol.mass; }
        else { damage *= 5; } //if object down on ground
    }

}
