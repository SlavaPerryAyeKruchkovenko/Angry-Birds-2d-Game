using Assets.scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public GameObject SelectedBird { get; private set; }
    private GameObject slingshot;
    public Bird Bird { get; private set; }
    public Queue<GameObject> Birds { get; private set; } = new Queue<GameObject>();
    public bool GameStart { get; private set;} = false;
    public Vector3 StartLocation { get; private set; } = default;
    private Quaternion startRotation = default;

    private bool isPress;
    // Start is called before the first frame update
    void Awake()
    {
        if(slingshot == null)
		{
            slingshot = GameObject.Find("Slingshot");
            var position = new Vector3(slingshot.transform.position.x, slingshot.transform.position.y, -1);
            StartLocation = position;

            var birds = GameObject.FindGameObjectsWithTag("Bird");
            foreach (var item in birds)
            {
                Birds.Enqueue(item);
            }
        }          
    }

    // Update is called once per frame
    void Update()
    {
        var coor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mouseCoor = new Vector3(coor.x, coor.y, -1);
        if (GameStart && SelectedBird != null && !Bird.IsFly)
        {
            var cameraLeftButtomCoor = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)) + new Vector3(0, 1, 0);
            var cameraRightUpCoor = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)) - new Vector3(3, 1, 0);
            if (!CompareVectors3(mouseCoor, cameraLeftButtomCoor) || !CompareVectors3(cameraRightUpCoor,mouseCoor)) 
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
                    Bird.InvokeFlyEvent(StartLocation - mouseCoor);
                    if(Camera.main.GetComponent<MainCameraScript>())
					{
                        Camera.main.GetComponent<MainCameraScript>().AddBird(SelectedBird);
                        Camera.main.GetComponent<MainCameraScript>().LockCamera = false;
                    }
                    //Make Fly Animation;
                }
                else
				{
                    Bird.InvokeResetEvent();
                }
                
            }
            else if(isPress)
			{
                Camera.main.GetComponent<MainCameraScript>().LockCamera = true;
                Bird.InvokeTakeAimEvent(mouseCoor);
                var script = SelectedBird.GetComponent<BirdScript>();
                float impulse = GetImpulse(StartLocation - mouseCoor);
                script.DrawTraectory(SelectedBird.transform.right * impulse);
            }           
        }       
    }   
    private void DropBird(Vector3 range)//when Bird Start Flying
	{
        SelectedBird.AddComponent<Rigidbody2D>();
        var rigidbody = SelectedBird.GetComponent<Rigidbody2D>();
        rigidbody.mass = Bird.Mass;
        var power = GetImpulse(range);
        var vector = SelectedBird.transform.right;
        rigidbody.AddForce(vector*power, ForceMode2D.Impulse);
	}
    private float GetImpulse(Vector3 power)
	{
        return Vector3.SqrMagnitude(power) / 2 * 1.6f;//delta x^2 * k /2
    }
    private void ChangeBand(Vector3 mouseCoordinate)
	{
        slingshot.GetComponent<LineRenderer>().enabled = true;
        var rangeVector = new Vector3(StartLocation.x - 0.3f, StartLocation.y + 0.1f, -1);
        slingshot.GetComponent<LineRenderer>().SetPositions(new Vector3[] { StartLocation, rangeVector, mouseCoordinate });
    }
    //Bird will be on mouse position with small delay
    private void ManageBird(Vector3 mouseCoordinate)
	{
        SelectedBird.transform.position = Vector3.MoveTowards(SelectedBird.transform.position,
                       new Vector2(mouseCoordinate.x, mouseCoordinate.y),
                       Time.deltaTime * 100);

        var vector = new Vector2(StartLocation.x, StartLocation.y);
        var range = vector - new Vector2(SelectedBird.transform.position.x, SelectedBird.transform.position.y);
        SelectedBird.transform.rotation = Quaternion.AngleAxis(GetAngle(range), Vector3.forward);
    } 
    public void ChangeBird()
	{       
        if (Birds.Count > 0) 
		{
            SelectedBird = Birds.Dequeue();
            Bird = SelectedBird.GetComponent<GameObjectScript>().ABGameObj as Bird;            
            Bird.TakeAim += ChangeBand;
            Bird.TakeAim += ManageBird;
            Bird.ResetBird += ResetBand;
            Bird.ResetBird += ResetBird;
            Bird.ReadyFly += DropBird;
            Bird.ResetBird += () => SelectedBird.GetComponent<LineRenderer>().enabled = false;
            Bird.TakeAim += (vector) => { SelectedBird.GetComponent<LineRenderer>().enabled = true; };
            startRotation = SelectedBird.transform.rotation;
        }
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
    public void StartGame() => this.GameStart = true;
}

