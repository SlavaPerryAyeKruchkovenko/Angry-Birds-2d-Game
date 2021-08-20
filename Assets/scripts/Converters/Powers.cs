using Assets.scripts.Angry_Birds_2d_BusnesLogic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.scripts.Converters
{
	class Powers : IPowers
	{
		public Powers(GameObject _gameObject)
		{
			gameObject = _gameObject;
		}
		private readonly GameObject gameObject;
		public async void Clone(CancellationTokenSource cancelTokenSource)
		{
			if(cancelTokenSource.IsCancellationRequested)
			{
				return;
			}
			var gameObj1 = GameObject.Instantiate(gameObject, gameObject.transform.position + Vector3.up*2, default);
			ReturnForce(gameObj1, gameObject);
			var gameObj2 = GameObject.Instantiate(gameObject, gameObject.transform.position - Vector3.up*2, default);
			ReturnForce(gameObj2, gameObject);

			cancelTokenSource.Cancel();
			await Task.Delay(20);
			Debug.Log("клонируюсь");
		}
		private static void ReturnForce(GameObject gameObject1, GameObject gameObject2)
		{
			gameObject1.GetComponent<Rigidbody2D>().velocity = gameObject2.GetComponent<Rigidbody2D>().velocity;
			var game = gameObject1.GetComponent<GameObjectScript>();
			game.BirdType = Birds.BlueClone;
			game.SetStartSettings();

			gameObject1.SetActive(true);
		}
		public async void DropEgg(CancellationTokenSource cancelTokenSource)
		{
			if(cancelTokenSource.IsCancellationRequested)
			{
				return;
			}
			GameObject egg = GetEgg(gameObject);
			Object.Instantiate(egg, gameObject.transform.position - Vector3.up, default);			
			await Task.Delay(100);
			AddPower(egg, gameObject);
			cancelTokenSource.Cancel();
			Debug.Log("Снес яйцо");
		}
		private static GameObject GetEgg(GameObject game)
		{		
			var egg = game.transform.Find("egg").gameObject;
			egg.GetComponent<CapsuleCollider2D>().enabled = true;
			egg.GetComponent<SpriteRenderer>().enabled = true;
			return egg;
		}
		private static void AddPower(GameObject egg, GameObject gameObject)
		{
			var rigidbody = egg.GetComponent<Rigidbody2D>();
			rigidbody.velocity = Vector2.zero;
			float power = rigidbody.mass * 10;//F = mg
			
			rigidbody.AddForce(-Vector3.up * power, ForceMode2D.Impulse);
			gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 10, ForceMode2D.Impulse);
		}
		public async void Explode(CancellationTokenSource cancelTokenSource)
		{
			if(gameObject.GetComponent<CircleCollider2D>())
			{
				var colider = gameObject.GetComponent<CircleCollider2D>();
				var rigidbody = gameObject.GetComponent<Rigidbody2D>();
				
				float radius = colider.radius;
				float mass = rigidbody.mass;
				rigidbody.velocity -= new Vector2(rigidbody.velocity.x * 0.9f, rigidbody.velocity.y * 0.9f);
				for (float i = 1; i <= 1.5f; i += 0.1f)
				{
					colider.radius = radius * i;
					rigidbody.mass *= mass * i;
					await Task.Delay(250);
				}
				cancelTokenSource.Cancel();				
				gameObject.GetComponent<GameObjectScript>().ABGameObj.InvokeDiedEvent();
				Debug.Log("boom");
			}
		} 

		public async void SpeedUp(CancellationTokenSource cancelTokenSource)
		{
			var rigidbody = gameObject.GetComponent<Rigidbody2D>();
			Vector2 startSpeed = rigidbody.velocity;
			float speed;
			for (speed = 1.5f; speed < 2f; speed += 0.1f)
			{
				rigidbody.velocity = startSpeed * speed;
				rigidbody.mass += 1;
				if (cancelTokenSource.IsCancellationRequested)
				{
					return;
				}
				await Task.Delay(84);
			}
			cancelTokenSource.Cancel();
		}

		public async void UTurn(CancellationTokenSource cancelTokenSource)
		{
			CancellationToken token = cancelTokenSource.Token;
			var rotation = gameObject.transform.rotation;
			var rigidbody = gameObject.GetComponent<Rigidbody2D>();
			Vector2 speed = rigidbody.velocity;			
			float angel = gameObject.transform.rotation.eulerAngles.z;
			for (int i = 1; i <= 180; i+=3)
			{
				if(cancelTokenSource.Token.IsCancellationRequested)//if bird die
				{
					return;
				}
				gameObject.transform.rotation = Quaternion.AngleAxis(angel - i, Vector3.forward);
				if (rigidbody.velocity.magnitude > 1)
				{
					rigidbody.velocity -= new Vector2(0.03f, 0.03f);
				}
				await Task.Delay(1);
			}
			cancelTokenSource.Cancel();
			await Task.Run(() => Debug.Log("Развернулся"));
			rigidbody.velocity = new Vector2(-speed.x , speed.y);
		}		
	}
}
