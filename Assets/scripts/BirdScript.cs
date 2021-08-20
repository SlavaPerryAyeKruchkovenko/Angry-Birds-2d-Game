using Assets.scripts;
using Assets.scripts.Angry_Birds_2d_BusnesLogic;
using Assets.scripts.Exstensions;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

internal class BirdScript : MonoBehaviour
{
	public GameObject FlyMaterial;
	private Bird bird;
	private bool canDrawPoint = true;
	private readonly int steps = 50;
	private const float g = 9.8f;
	public readonly CancellationTokenSource Token = new CancellationTokenSource();
	private Animator animator;

	public void Awake()
	{		
		var game = GetComponent<GameObjectScript>();
		if (bird == null && game)
		{
			animator = GetComponent<Animator>();
			bird = game.ABGameObj as Bird;
			
			if(bird != null)
			{
				AwakeStartSetting();
			}			
		}
	}
	internal async void DrawPoints(CancellationTokenSource token)
	{
		for (int i = 0; i <= 100; i++)
		{
			if (token.Token.IsCancellationRequested)
			{
				return;
			}
			if (GetComponent<Rigidbody2D>())
			{
				Instantiate(FlyMaterial, transform.position, default);
			}
			await Task.Delay(5);
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (bird != null && canDrawPoint && collision.CompareTag("Slingshot"))
		{
			DrawPoints(Token);
			canDrawPoint = false;
		}
	}
	private void OnCollisionEnter2D(Collision2D collision)//use ability if bird touch game object
	{
		if (bird is IBird birdAbility && GetComponent<Rigidbody2D>())
		{
			if (birdAbility.AbilityType == TypeUsingAbility.TouchObject || birdAbility.AbilityType == TypeUsingAbility.Universal)
			{			
				Token.Cancel();
				birdAbility.UsePower();
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
		if (collision.CompareTag("Background") && GetComponent<Rigidbody2D>())
		{
			if (Input.GetMouseButton(0))
			{
				if (bird is IBird ibird && (ibird.AbilityType == TypeUsingAbility.Click || ibird.AbilityType == TypeUsingAbility.Universal))
				{
					Token.Cancel();
					ibird.UsePower();
				}
			}
		}
	}
	internal void DrawTraectory(System.Numerics.Vector3 range)
	{
		var coordinate = CountPoints(transform.position, range.ConvertBaseVectorInUnity());
		GetComponent<LineRenderer>().SetPositions(coordinate);
	}
	private Vector3[] CountPoints(Vector3 posicion, Vector3 impulse)
	{
		Vector3[] results = new Vector3[steps];
		Vector2 newImpulse = new Vector2(impulse.x, impulse.y);
		float speed = (newImpulse / bird.Mass).magnitude; // v = p/m
		float corner = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;

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
	private void SetAblityAnimation(CancellationTokenSource token)
	{
		animator.SetBool("UseAbility", true);
	}
	public void ChangeParticles()
	{
		var system = gameObject.GetComponent<ParticleSystem>();
		if (system) 
		{
			if(system.isPlaying)
			{
				system.Stop();
			}
			else if(bird!= null && bird.Health > 0)
			{
				system.Play(false);
			}			
		}			
	}
	public void ChangeDieParticles()
	{
		if(transform.childCount > 0)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				var system = gameObject.transform.GetChild(i).gameObject;
				var particleSystem = system.GetComponent<ParticleSystem>();
				if (particleSystem)
				{
					if(particleSystem.isPlaying)
					{
						particleSystem.Stop();
					}
					else
					{
						particleSystem.Play(true);
					}
				}
			}		
		}	
	}
	private void AwakeStartSetting()
	{
		var lineRenderer = GetComponent<LineRenderer>();
		if (lineRenderer)
		{
			lineRenderer.positionCount = steps;
			bird.StartFly += () => Destroy(lineRenderer);
		}
		if (canDrawPoint)
		{
			bird.StartFly += ChangeParticles;
			bird.StartFly += DeleteFlyPoints;
			bird.TakeAim += DrawTraectory;
			bird.ObjectGetDamage += Token.Cancel;
			if (animator)
			{
				animator.enabled = true;
				bird.ObjectGetDamage += () => animator.SetBool("IsFly", false);
				bird.StartFly += () => animator.SetBool("IsFly", bird.IsFly);
				if (bird is BirdWithPower ibird)
				{
					ibird.Ability += SetAblityAnimation;
					if (ibird.AbilityType == TypeUsingAbility.TouchObject || ibird.AbilityType == TypeUsingAbility.Universal)
					{
						bird.ObjectDie += ibird.UsePower;
					}
				}
				if (bird is BlueBird blueBird && blueBird.IsClone)
				{
					animator.SetBool("IsClone", true);
					animator.SetBool("IsFly", true);
				}
				if (bird is WhiteBird)
				{
					var egg = gameObject.transform.Find("egg");
					var script = egg.GetComponent<GameObjectScript>();
					if (script != null)
					{
						script.ABGameObj.ObjectGetDamage += ChangeDieParticles;
					}
				}
			}
		}
	}
}
