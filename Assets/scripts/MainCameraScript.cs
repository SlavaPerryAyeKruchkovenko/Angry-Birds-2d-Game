using Assets.scripts;
using System;
using UnityEngine;

public class MainCameraScript : MonoBehaviour, IObserver<GameObject>
{
	[SerializeField]
	private const short maxCameraSize = 10;
	[SerializeField]
	private const short minCameraSize = 6;
	private const float clickTime = 0.12f;//0.12 is time of standart click
	public GameObject FlyingBird { get; private set; }
	public bool NeedCheck { get; private set; }

	private Bird bird;
	private GameObject background;
	private Vector3 startPosition;
	private Vector3 lastLocation;
	private Vector3? range = null;
	private float firstClickTime = 0;
	private int clickCount = 0;
	private bool lockCamera = false;
	// Start is called before the first frame update
	void Awake()
	{
		if (background) return;
		background = GameObject.FindGameObjectWithTag("Background");
		startPosition = transform.position;	
	}

	// Update is called once per frame
	void Update()
	{
		if (!lockCamera)
		{
			if (Input.mouseScrollDelta.y != 0)
			{
				SelectCameraChanges();
			}
			lastLocation = transform.position;
			if (CheckOnDoubleTap())//if user use double click skip range
			{
				ResetCamera();
				if(FlyingBird && FlyingBird.GetComponent<Rigidbody2D>())
				{
					bird.ObjectDie -= ResetCamera;
				}
				ChangeCameraSize(Camera.main, -(Camera.main.orthographicSize - minCameraSize));
			}
			else if (Input.GetMouseButtonDown(0))
			{
				if (range == null)
				{
					range = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				}
				else
				{
					range += Camera.main.ScreenToWorldPoint(Input.mousePosition);
				}
			}
			else if (Input.GetMouseButtonUp(0))
			{
				if (range != null)
				{
					range -= Camera.main.ScreenToWorldPoint(Input.mousePosition);
					transform.position += new Vector3(range.Value.x, range.Value.y, 0);
				}
				if (!CanMoveCamera(background, Camera.main))
				{
					transform.position = lastLocation;
				}
				range = null;
			}
			else if (bird != null && FlyingBird && bird.IsFly && NeedCheck)
			{
				var coor = FlyingBird.transform.position;
				transform.position = new Vector3(coor.x, coor.y, transform.position.z);
				if (!CanMoveCamera(background, Camera.main))
				{
					gameObject.transform.position = lastLocation;
				}
			}
		}
		else
		{
			range = null;
		}
	}
	private bool CheckOnDoubleTap()
	{
		if (clickCount >= 2)
		{
			clickCount = 0;
			return true;
		}
		else if (Input.GetMouseButtonDown(0))
		{
			firstClickTime = Time.time;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			if (Time.time - firstClickTime < clickTime)
			{
				clickCount++;
			}
			else
			{
				clickCount = 0;
			}
		}
		return false;
	}
	public void OnCompleted()
	{
		throw new NotImplementedException();
	}

	public void OnError(Exception error)
	{
		Debug.Log(error.Message);
	}

	public void OnNext(GameObject value)
	{
		var script = value.GetComponent<GameObjectScript>();
		if (script && script.ABGameObj is Bird)
		{
			FlyingBird = value;
			bird = value.GetComponent<GameObjectScript>().ABGameObj as Bird;
			bird.ObjectDie += ResetCamera;
			NeedCheck = true;
			bird.StartFly += () => lockCamera = false;
			bird.TakeAim += (vector) => lockCamera = true;
		}
	}
	private void ResetCamera()
	{
		transform.position = startPosition;
		if (FlyingBird && FlyingBird.GetComponent<Rigidbody2D>())
			NeedCheck = false;
	}
	private bool SelectCameraChanges()
	{
		if (Input.mouseScrollDelta.y < 0.0f && Camera.main.orthographicSize < maxCameraSize)
			ChangeCameraSize(Camera.main, 1);

		else if (Input.mouseScrollDelta.y > 0.0f && Camera.main.orthographicSize > minCameraSize)
			ChangeCameraSize(Camera.main, -1);

		else
			return false;

		return true;
	}
	private void ChangeCameraSize(Camera camera, float value)
	{
		float startValue = Mathf.Abs(camera.ViewportToWorldPoint(new Vector2(0, 0)).x);
		camera.orthographicSize += value;

		float finishValue = Mathf.Abs(camera.ViewportToWorldPoint(new Vector2(0, 0)).x);//left point after change size
		var range = (finishValue - startValue) * Vector3.right;
		camera.transform.position = startPosition + range;

		if (!CanMoveCamera(background, camera))
		{
			camera.orthographicSize -= value;
			camera.transform.position -= range;
		}
		else
		{
			startPosition += range;
		}
	}
	private static bool CanMoveCamera(GameObject background, Camera camera)
	{
		var rect = background.GetComponent<RectTransform>();
		float width = background.transform.localScale.x * rect.rect.width; // wight
		float height = background.transform.localScale.y * rect.rect.height;// height
		Vector2 backgroundCoor = background.transform.position;
		float leftX = backgroundCoor.x - width / 2; //left x background coor
		float rightX = backgroundCoor.x + width / 2; //right x background coor
		float upY = backgroundCoor.y + height / 2;// up Y background coor
		float buttomY = backgroundCoor.y - height / 2;// buttom Y background coor
		var cameraLeftButtomCoor = camera.ViewportToWorldPoint(new Vector2(0, 0));
		var cameraRightUpCoor = camera.ViewportToWorldPoint(new Vector2(1, 1));
		return leftX < cameraLeftButtomCoor.x &&
			buttomY < cameraLeftButtomCoor.y &&
			rightX > cameraRightUpCoor.x &&
			upY > cameraRightUpCoor.y;
	}
}
