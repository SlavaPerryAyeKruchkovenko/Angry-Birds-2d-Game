using Assets.scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public GameObject SelectedBird { get; private set; }
    private GameObject Slingshot;
    public Queue<GameObject> Birds { get; private set; } = new Queue<GameObject>();
    public bool GameStart { get; private set;} = false;
    public Vector3 StartLocation = default;
    private Quaternion startRotation = default;

    private bool isPress;
    private float xLimit;
    // Start is called before the first frame update
    void Start()
    {
        Slingshot = GameObject.Find("Slingshot");

        xLimit = GameObject.Find("Slingshot").transform.position.x + 3;

        var birds = GameObject.FindGameObjectsWithTag("Bird");
		foreach (var item in birds)
		{
            Birds.Enqueue(item);
		}       
    }

    // Update is called once per frame
    void Update()
    {
        var coor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mouseCoor = new Vector3(coor.x, coor.y, -1);
        if (GameStart && SelectedBird != null)
        {                   
            if(mouseCoor.x >= xLimit)
			{
                isPress = false;
                ResetBird();
                ResetBand();
            }
            else if (Input.GetMouseButtonDown(0))
            {
                isPress = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isPress = false;
                ResetBand();
                if (!SelectedBird.GetComponent<Rigidbody2D>())
                {
                    SelectedBird.AddComponent<Rigidbody2D>();
                    GameObject.Find("Background").GetComponent<Class1>().GameObject = SelectedBird;
                    SelectedBird.GetComponent<Rigidbody2D>().useAutoMass = true;
                    DropBird(SelectedBird, StartLocation - mouseCoor);
                    SelectedBird = null;
                }
            }
            else if(isPress)
			{
                ManageBird(mouseCoor, SelectedBird , StartLocation);
                ChangeBand(mouseCoor, Slingshot, StartLocation);
            }
            
        }       
    }
    private void ResetBird()//ResetPosition
	{
        SelectedBird.transform.position = StartLocation;
        SelectedBird.transform.rotation = startRotation;
    }
    private void ResetBand()
	{
        Slingshot.GetComponent<LineRenderer>().enabled = false;
    }
    private static void DropBird(GameObject bird , Vector3 range)
	{
        var power = Vector3.SqrMagnitude(range) / 2 * 0.8f;//delta x^2 * k /2
        bird.GetComponent<Rigidbody2D>().AddForce(bird.transform.right*power, ForceMode2D.Impulse);
        bird.GetComponent<BirdScript>().StartFlying();
	}
    private static void ChangeBand(Vector3 mouseCoordinate , GameObject slingshot , Vector2 startLocation)
	{
        slingshot.GetComponent<LineRenderer>().enabled = true;
        var rangeVector = new Vector3(startLocation.x - 0.3f, startLocation.y + 0.1f, -1);
        slingshot.GetComponent<LineRenderer>().SetPositions(new Vector3[] { startLocation, rangeVector, mouseCoordinate });
    }
    private static void ManageBird(Vector3 mouseCoordinate , GameObject selectedBird , Vector2 startLocation)
	{
        selectedBird.transform.position = Vector3.MoveTowards(selectedBird.transform.position,
                       new Vector2(mouseCoordinate.x, mouseCoordinate.y),
                       Time.deltaTime * 100);

        var range = startLocation - new Vector2(selectedBird.transform.position.x, selectedBird.transform.position.y);
        selectedBird.transform.rotation = Quaternion.AngleAxis(GetAngle(range), Vector3.forward);
    }
    private static float GetAngle(Vector3 vector) => Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
    public void ChangeGameConditional() => this.GameStart = !this.GameStart;
    public void ChangeBird()
	{
        if (Birds.Count > 0) 
		{
            SelectedBird = Birds.Dequeue();
            startRotation = SelectedBird.transform.rotation;
        }
	}
    
}

