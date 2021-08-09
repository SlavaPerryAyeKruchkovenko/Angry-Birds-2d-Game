using Assets.scripts;
using Assets.scripts.Angry_Birds_2d_BusnesLogic;
using System.Linq;
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
	private CancellationTokenSource token = new CancellationTokenSource();

	private void Awake()
	{				
		gameObject.GetComponent<LineRenderer>().positionCount = steps;
		if(bird == null)
		{
			bird = this.gameObject.GetComponent<GameObjectScript>().ABGameObj as Bird;
			if (bird != null && canDrawPoint)
			{
				bird.StartFly += DeleteFlyPoints;				
			}
		}
		if (bird.cancelTokenSource.IsCancellationRequested)
		{
			token.Cancel();
		}
	}
	async public void DrawPoints(CancellationTokenSource token)
	{
		for (int i = 0; i <= 100; i++)
		{
			if(token.Token.IsCancellationRequested)
			{
				return;
			}
			if (this.gameObject.GetComponent<Rigidbody2D>())
			{
				Instantiate(FlyMaterial, this.gameObject.transform.position, default);			
			}
			await Task.Delay(5);
		}	
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (bird!=null && canDrawPoint && collision.gameObject.CompareTag("Slingshot"))
		{
			DrawPoints(token);
			canDrawPoint = false;
		}
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (bird is IBird birdAbility)
		{
			if (birdAbility.Ability == TypeUsingAbility.TouchObject || birdAbility.Ability == TypeUsingAbility.Universal)
			{				
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
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Background") && this.gameObject.GetComponent<Rigidbody2D>())
		{
			if (bird is IBird ibird && ibird.Ability == TypeUsingAbility.Click)
			{
				token.Cancel();
				ibird.UsePower();
			}
		}
	}
	public void DrawTraectory(Vector3 range)
	{
		var coordinate = CountPoints(this.gameObject.transform.position, range);
		this.gameObject.GetComponent<LineRenderer>().SetPositions(coordinate);
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
			float y = speed * Mathf.Sin(corner) * time - g * Mathf.Pow(time, 2) / 2; //y = v0 * sina * t - g * t^2 / 2
			Vector3 point = new Vector2(x, y);
			results[i] = posicion + point;
		}
		return results;
	}
}
