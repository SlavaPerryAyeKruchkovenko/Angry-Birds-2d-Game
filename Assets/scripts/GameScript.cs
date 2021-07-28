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
    // Start is called before the first frame update
    void Start()
    {
        Slingshot = GameObject.Find("Slingshot");

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
            var cameraLeftButtomCoor = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)) + new Vector3(0, 1, 0);
            var cameraRightUpCoor = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)) - new Vector3(3, 1, 0);
            if (!CompareVectors3(mouseCoor, cameraLeftButtomCoor) || !CompareVectors3(cameraRightUpCoor,mouseCoor)) 
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
    private static void DropBird(GameObject bird , Vector3 range)//when Bird Start Flying
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
    public static bool CompareVectors3(Vector3 vector1, Vector3 vector2)//if 1 postion will be more that true
	{
        if (vector1.x > vector2.x && vector1.y > vector2.y)
            return true;
        else
            return false;
	}
    
}

