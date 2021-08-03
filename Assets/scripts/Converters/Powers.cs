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
		public void Clone()
		{
			Debug.Log("клонируюсь");
		}

		public void DropEgg()
		{
			Debug.Log("Снес яйцо");
		}

		public void Explode()
		{
			if(gameObject.GetComponent<CircleCollider2D>())
			{
				var colider = gameObject.GetComponent<CircleCollider2D>();
				var rigidbody = gameObject.GetComponent<Rigidbody2D>();
				rigidbody.velocity = Vector2.zero;
				colider.radius *= 1.5f;
				rigidbody.mass *= 1.5f;
				Debug.Log("boom");
			}
		} 

		public void SpeedUp()
		{
			gameObject.GetComponent<Rigidbody2D>().velocity *= 3;
		}

		async public void UTurn()
		{
			var rotation = gameObject.transform.rotation;
			var rigidbody = gameObject.GetComponent<Rigidbody2D>();
			Vector2 speed = rigidbody.velocity;			
			float angel = gameObject.transform.rotation.eulerAngles.z;
			for (int i = 1; i <= 180; i+=3)
			{
				gameObject.transform.rotation = Quaternion.AngleAxis(angel - i, Vector3.forward);
				if(rigidbody.velocity.magnitude>0)
				{
					rigidbody.velocity -= new Vector2(0.03f, 0.03f);
				}				
				await Task.Delay(1);
			}
			await Task.Run(() => Debug.Log("Развернулся"));
			rigidbody.velocity = -speed;
		}
	}
}
