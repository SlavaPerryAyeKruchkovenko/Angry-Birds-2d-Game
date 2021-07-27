using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class BirdScript : MonoBehaviour
{
    public List<Sprite> FlyingSprites;
    public List<Sprite> PowerSprites;
    public GameObject ExtraObject;
    private Action StartPower { get; set; }
    private bool IsPowerActivated { get; set; } = false;

    void Start()
    {

    }

    void Update()
    {

    }

    public async void StartFlying()
    {
        foreach (var sprite in FlyingSprites)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
            await Task.Delay(700);
            if (IsPowerActivated) break;
        }
    }

    public async void ActivatePower()
    {
        foreach(var sprite in PowerSprites)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
            if (sprite == PowerSprites[PowerSprites.Count - 1]) await Task.Delay(700);
        }
        StartPower();
    }

    private void BlueBirdPower() 
    {
        Instantiate(ExtraObject, new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y - 1), gameObject.transform.rotation);
        Instantiate(ExtraObject, new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y - 1), gameObject.transform.rotation);
    }


    private async void YellowBirdPower() 
    {
        var rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        while (gameObject.GetComponent<GameObjectScript>().GameObj.Health > 0) 
        {
            var startX = rigidbody2D.velocity.x;
            var startY = rigidbody2D.velocity.y;
            rigidbody2D.velocity = new Vector2(startX * (float)1.5, startY * (float)1.5);
            await Task.Delay(1000);
        }
    }

    private async void BlackBirdPower() 
    {
        var collider = gameObject.GetComponent<CircleCollider2D>();
        var rigidbody = gameObject.GetComponent<Rigidbody2D>();
        rigidbody.mass = 100;
        collider.radius *= 3;
        await Task.Delay(100);
        Destroy(gameObject);
    }

    private void GreenBirdPower()
    {
        var angle = Mathf.Atan2(gameObject.transform.position.y, gameObject.transform.position.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void WhiteBirdPower()
    {
        Instantiate(ExtraObject, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 10), gameObject.transform.rotation);
    }
}
