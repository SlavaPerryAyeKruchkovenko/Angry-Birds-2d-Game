using Assets.scripts;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
	public GameObject FlyMaterial;
	private Bird bird;
	private bool canDrawPoint = true;
	private void Awake()
	{
		bird = (Bird)this.gameObject.GetComponent<GameObjectScript>().ABGameObj;
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
				await Task.Delay(5);
			}
		}	
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (canDrawPoint && collision.gameObject.CompareTag("Slingshot"))
		{
			DrawPoints(bird.cancelTokenSource);
			canDrawPoint = false;
		}
	}
}
