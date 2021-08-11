using Assets.scripts.Angry_Birds_2d_BusnesLogic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.scripts.Converters
{
	class Powers : IPowers
	{
		private readonly GameObject gameObject;

		public Powers(GameObject _gameObject)
		{
			gameObject = _gameObject;
		}

		public async void Clone(CancellationTokenSource cancelTokenSource)
		{
			if(cancelTokenSource.IsCancellationRequested) return;
			var gameObj1 = GameObject.Instantiate(gameObject, gameObject.transform.position + Vector3.up, default);
			ReturnForce(gameObj1, gameObject);
			var gameObj2 = GameObject.Instantiate(gameObject, gameObject.transform.position - Vector3.up, default);
			ReturnForce(gameObj2, gameObject);

			cancelTokenSource.Cancel();
			await Task.Delay(20);
			Debug.Log("клонируюсь");
		}
		private static void ReturnForce(GameObject gameObject1, GameObject gameObject2)
		{
			gameObject1.GetComponent<Rigidbody2D>().velocity = gameObject2.GetComponent<Rigidbody2D>().velocity;
			gameObject1.GetComponent<GameObjectScript>().BirdType = Birds.BlueClone;
			gameObject1.GetComponent<GameObjectScript>().SetStartSettings();
		}
		public async void DropEgg(CancellationTokenSource cancelTokenSource)
		{
			if(cancelTokenSource.IsCancellationRequested) return;
			GameObject egg = GetEgg(gameObject);
			GameObject.Instantiate(egg, this.gameObject.transform.position - Vector3.up, default);			
			await Task.Delay(20);
			AddPower(egg, gameObject);
			cancelTokenSource.Cancel();
			Debug.Log("Снес яйцо");
		}
		private static GameObject GetEgg(GameObject game)
		{
			var egg = game.transform.GetChild(0).gameObject;
			egg.GetComponent<CapsuleCollider2D>().enabled = true;
			egg.GetComponent<SpriteRenderer>().enabled = true;
			return egg;
		}
		private static void AddPower(GameObject egg, GameObject gameObject)
		{
			var rigidbody = egg.GetComponent<Rigidbody2D>();
			float power = rigidbody.mass * 10;//F = mg
			rigidbody.AddForce(-Vector3.up * power, ForceMode2D.Impulse);
			gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 10, ForceMode2D.Impulse);
		}
		async public void Explode(CancellationTokenSource cancelTokenSource)
		{
			if(gameObject.GetComponent<CircleCollider2D>())
			{
				var colider = gameObject.GetComponent<CircleCollider2D>();
				var rigidbody = gameObject.GetComponent<Rigidbody2D>();
				float radius = colider.radius;
				float mass = rigidbody.mass;
				rigidbody.velocity -= new Vector2(rigidbody.velocity.x * 0.8f, rigidbody.velocity.y * 0.8f);
				for (float i = 1; i < 3; i += 0.1f)
				{
					colider.radius = radius * i;
					rigidbody.mass *= mass * i;
					if (cancelTokenSource.IsCancellationRequested)
					{
						gameObject.GetComponent<GameObjectScript>().ABGameObj.InvokeDiedEvent();
						return;						
					}
					await Task.Delay(10);
				}
				cancelTokenSource.Cancel();				
				gameObject.GetComponent<GameObjectScript>().ABGameObj.InvokeDiedEvent();
				Debug.Log("boom");
			}
		}

		public async void SpeedUp(CancellationTokenSource cancelTokenSource)
		{
			Vector2 startSpeed = gameObject.GetComponent<Rigidbody2D>().velocity;
			for (int speed = 1; speed <= 3; speed++)
			{
				var rigidBody = gameObject.GetComponent<Rigidbody2D>();
				rigidBody.velocity = startSpeed * speed;
				rigidBody.mass += 1;
				if (cancelTokenSource.IsCancellationRequested)
				{
					return;
				}
				await Task.Delay(10);
			}
			cancelTokenSource.Cancel();
		}

		async public void UTurn(CancellationTokenSource cancelTokenSource)
		{
			CancellationToken token = cancelTokenSource.Token;
			var rotation = gameObject.transform.rotation;
			var rigidBody = gameObject.GetComponent<Rigidbody2D>();
			Vector2 speed = rigidBody.velocity;			
			float angel = gameObject.transform.rotation.eulerAngles.z;
			for (int i = 1; i <= 180; i+=3)
			{
				if(cancelTokenSource.Token.IsCancellationRequested)//if bird die
				{
					return;
				}
				gameObject.transform.rotation = Quaternion.AngleAxis(angel - i, Vector3.forward);
				if (rigidBody.velocity.magnitude > 1)
				{
					rigidBody.velocity -= new Vector2(0.03f, 0.03f);
				}
				await Task.Delay(1);
			}
			cancelTokenSource.Cancel();
			await Task.Run(() => Debug.Log("Развернулся"));
			rigidBody.velocity = -speed;
		}		
	}
}
