using Assets.scripts;
using Assets.scripts.Angry_Birds_2d_BusnesLogic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
	public GameObject FlyMaterial;
	private Bird bird;
	private bool canDrawPoint = true;
	private readonly int steps = 50;
	private const float g = 9.8f;
	public readonly CancellationTokenSource Token = new CancellationTokenSource();

	private void Awake()
	{
		if (bird == null)
		{
			bird = gameObject.GetComponent<GameObjectScript>().ABGameObj as Bird;
			if (gameObject.GetComponent<LineRenderer>())
			{
				gameObject.GetComponent<LineRenderer>().positionCount = steps;
				bird.StartFly += () => Destroy(gameObject.GetComponent<LineRenderer>());
			}
			if (bird != null && canDrawPoint)
			{
				bird.StartFly += DeleteFlyPoints;
			}
		}
	}
	async public void DrawPoints(CancellationTokenSource token)
	{
		for (int i = 0; i <= 100; i++)
		{
			if (token.Token.IsCancellationRequested)
			{
				return;
			}
			if (gameObject.GetComponent<Rigidbody2D>())
			{
				Instantiate(FlyMaterial, gameObject.transform.position, default);
			}
			await Task.Delay(5);
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (bird != null && canDrawPoint && collision.gameObject.CompareTag("Slingshot"))
		{
			DrawPoints(Token);
			canDrawPoint = false;
		}
	}
	private void OnCollisionEnter2D(Collision2D collision)//use ability if bird touch game object
	{
		if (bird is IBird birdAbility)
		{
			if (birdAbility.Ability == TypeUsingAbility.TouchObject || birdAbility.Ability == TypeUsingAbility.Universal)
			{
				Token.Cancel();
				birdAbility.UsePower();
				bird.GetDamage(1);
			}
		}
	}
	private void DeleteFlyPoints()
	{
		foreach (var item in GameObject.FindGameObjectsWithTag("point"))
		{
			Destroy(item);
		}
	}
	private void OnTriggerStay2D(Collider2D collision)// if bird in game scnee triger can use ability
	{
		if (collision.CompareTag("Background") && gameObject.GetComponent<Rigidbody2D>())
		{
			if (Input.GetMouseButton(0))
			{
				if (bird is IBird ibird && ibird.Ability == TypeUsingAbility.Click)
				{
					Token.Cancel();
					ibird.UsePower();
				}
			}
		}
	}
	public void DrawTraectory(Vector3 range)
	{
		var coordinate = CountPoints(gameObject.transform.position, range);
		gameObject.GetComponent<LineRenderer>().SetPositions(coordinate);
	}
	private Vector3[] CountPoints(Vector3 posicion, Vector3 impulse)
	{
		Vector3[] results = new Vector3[steps];
		Vector2 newImpulse = new Vector2(impulse.x, impulse.y);
		float speed = (newImpulse / bird.Mass).magnitude; // v = p/m
		float corner = gameObject.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;

		float time = 0;
		for (int i = 0; i < steps; i++)
		{
			time += 0.1f;
			float x = speed * Mathf.Cos(corner) * time;// x = |v| * cos(a) * t
			float y = (speed * Mathf.Sin(corner) * time) - (g * Mathf.Pow(time, 2) / 2); //y = v0 * sina * t - g * t^2 / 2
			Vector3 point = new Vector2(x, y);
			results[i] = posicion + point;
		}
		return results;
	}
}
