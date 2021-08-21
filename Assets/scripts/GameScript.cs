using Assets.scripts;
using Assets.scripts.Exstensions;
using Assets.scripts.ViewModel;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

internal class GameScript : MonoBehaviour, IObserver<GameObject>
{
	public event Action StartGame = null;
	public GameObject SelectedBird { get; private set; }
	public Bird Bird { get; private set; }
	public Vector3 StartLocation { get; private set; } = default;
	public bool IsGameStart { get; private set; } = false;

	[SerializeField]
	private GameObject messageBox;

	private GameObject slingshot;
	private Quaternion startRotation = default;
	private bool isPress;
	// Start is called before the first frame update
	void Awake()
	{
		if (slingshot) return;
		if (messageBox)
		{
			messageBox.SetActive(false);
			GameViewModel.GameEnd += () => messageBox.SetActive(true);
			GameViewModel.GameEnd += () => IsGameStart = false;
		}
		StartGame += () => IsGameStart = true;
		slingshot = GameObject.Find("Slingshot");
		if (slingshot)
		{
			var position = new Vector3(slingshot.transform.position.x, slingshot.transform.position.y, -1);
			StartLocation = position;
		}
	}

	// Update is called once per frame
	void Update()
	{		
		if (IsGameStart && SelectedBird && !Bird.IsFly)
		{
			var coor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			var mouseCoor = new Vector3(coor.x, coor.y, -1);
			var cameraLeftButtomCoor = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)) + Vector3.up;
			var cameraRightUpCoor = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)) - new Vector3(3, 1, 0);
			if (!CompareVectors3(mouseCoor, cameraLeftButtomCoor) || !CompareVectors3(cameraRightUpCoor, mouseCoor))
			{
				isPress = false;
				Bird.InvokeResetEvent();
			}
			else if (Input.GetMouseButtonDown(0))
			{
				isPress = true;
			}
			else if (Input.GetMouseButtonUp(0))
			{
				isPress = false;
				ResetBand();
				if (!SelectedBird.GetComponent<Rigidbody2D>() && Mathf.Abs(StartLocation.x - mouseCoor.x) > 1)
				{
					var vector = (StartLocation - mouseCoor).ConvertUnityVectorInBase();
					Bird.InvokeFlyEvent(vector);
				}
				else
				{
					Bird.InvokeResetEvent();
				}

			}
			else if (isPress)
			{
				Bird.InvokeChangeBirdEvent(mouseCoor.ConvertUnityVectorInBase());
				float impulse = GetImpulse(StartLocation - mouseCoor);
				Bird.InvokeTakeAim((SelectedBird.transform.right * impulse).ConvertUnityVectorInBase());
			}
		}
	}
	private void DropBird(System.Numerics.Vector3 range)//when Bird Start Flying
	{
		var rigidbody = SelectedBird.AddComponent<Rigidbody2D>();
		rigidbody.mass = Bird.Mass;
		var power = GetImpulse(range.ConvertBaseVectorInUnity());
		var vector = SelectedBird.transform.right;
		rigidbody.AddForce(vector * power, ForceMode2D.Impulse);
	}
	private float GetImpulse(Vector3 power)
	{
		return Vector3.SqrMagnitude(power) / 2 * 1.6f;//delta x^2 * k /2
	}
	private void ChangeBand(System.Numerics.Vector3 mouseCoordinate)
	{
		var lineRenderer = slingshot.GetComponent<LineRenderer>();
		var rangeVector = new Vector3(StartLocation.x - 0.3f, StartLocation.y + 0.1f, -1);
		var mouseCoor = mouseCoordinate.ConvertBaseVectorInUnity();

		lineRenderer.enabled = true;
		lineRenderer.SetPositions(new Vector3[] { StartLocation, rangeVector, mouseCoor });
	}
	//Bird will be on mouse position with small delay
	private void ManageBird(System.Numerics.Vector3 mouseCoordinate)
	{
		SelectedBird.transform.position = Vector3.MoveTowards(SelectedBird.transform.position,
					   new Vector2(mouseCoordinate.X, mouseCoordinate.Y),
					   Time.deltaTime * 100);

		var vector = new Vector2(StartLocation.x, StartLocation.y);
		var range = vector - new Vector2(SelectedBird.transform.position.x, SelectedBird.transform.position.y);
		SelectedBird.transform.rotation = Quaternion.AngleAxis(GetAngle(range), Vector3.forward);
	}
	private void ResetTraectroy()
	{
		var lineRenderer = SelectedBird.GetComponent<LineRenderer>();
		if (lineRenderer)
			lineRenderer.enabled = false;
	}
	private static float GetAngle(Vector3 vector) => Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
	public static bool CompareVectors3(Vector3 vector1, Vector3 vector2)//if 1 postion will be more that true
	{
		if (vector1.x > vector2.x && vector1.y > vector2.y)
			return true;
		else
			return false;
	}
	private void ResetBird()//ResetPosition
	{
		SelectedBird.transform.position = StartLocation;
		SelectedBird.transform.rotation = startRotation;
	}
	private void ResetBand()
	{
		slingshot.GetComponent<LineRenderer>().enabled = false;
	}
	public void InvokeStartGame()
	{
		StartGame.Invoke();
	}
	public void OnNext(GameObject value)
	{
		SelectedBird = value;
		Bird = SelectedBird.GetComponent<GameObjectScript>().ABGameObj as Bird;
		Bird.ChangeBird += ChangeBand;
		Bird.ChangeBird += ManageBird;
		Bird.ResetBird += ResetBand;
		Bird.ResetBird += ResetBird;
		Bird.ReadyFly += DropBird;
		Bird.ResetBird += ResetTraectroy;
		var lineRenderer = SelectedBird.GetComponent<LineRenderer>();
		Bird.ChangeBird += (vector) => { if (lineRenderer) lineRenderer.enabled = true; };
		startRotation = SelectedBird.transform.rotation;
	}
	public void OnCompleted()
	{
		throw new NotImplementedException();
	}
	public void OnError(Exception error)
	{
		Debug.Log(error.Message);
	}
	public static IEnumerator OpenMenu()
	{
		var operation = SceneManager.LoadSceneAsync(0);
		if (!operation.isDone)
			yield return null;
	}
}

