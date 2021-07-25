using System;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public GameObject SelectedBird { get; set; }
    private GameObject Slingshot;
    public Stack<GameObject> Birds = new Stack<GameObject>();
    public bool GameStart { get; private set;} = false;
    public Vector2 StartLocation = default;
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
            Birds.Push(item);
		}
        SelectedBird = Birds.Pop();
        startRotation = SelectedBird.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        var coor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mouseCoor = new Vector3(coor.x, coor.y, -1);
        if (GameStart)
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
                }
            }
            else if(SelectedBird != null && isPress)
			{
                ManageBird(mouseCoor, SelectedBird , StartLocation);
                ChangeBand(mouseCoor, Slingshot, StartLocation);
            }
            
        }       
    }
    private void ResetBird()
	{
        SelectedBird.transform.position = StartLocation;
        SelectedBird.transform.rotation = startRotation;
    }
    private void ResetBand()
	{
        Slingshot.GetComponent<LineRenderer>().enabled = false;
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
        var angle = Mathf.Atan2(range.y, range.x) * Mathf.Rad2Deg;
        selectedBird.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    public void ChangeGameConditional() => this.GameStart = !this.GameStart;
}
