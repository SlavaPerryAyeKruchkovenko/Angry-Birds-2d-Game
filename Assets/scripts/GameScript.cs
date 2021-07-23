using System;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public static readonly Vector2 BeaitifulRange = new Vector2(0.15f, 0.35f);
    public GameObject SelectedBird { get; set; }
    private GameObject elasticBand;
    public Stack<GameObject> Birds = new Stack<GameObject>();
    public bool GameStart { get; private set;} = false;
    public Vector2 StartLocation = default;
    private Quaternion startRotation = default;
    private Vector3 startBandScale = default;
    private Quaternion startBandRotation = default;

    private bool isPress;
    private float xLimit;
    // Start is called before the first frame update
    void Start()
    {
        elasticBand = GameObject.Find("/Slingshot/elastic band");
        startBandScale = elasticBand.transform.localScale; 

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
        Vector3 mouseCoor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
            else if(SelectedBird != null && isPress)
			{
                ManageBird(mouseCoor, SelectedBird , StartLocation);
                ChangeBand(mouseCoor, elasticBand, StartLocation);

                if (Input.GetMouseButtonUp(0))
                {
                    isPress = false;
                    SelectedBird.AddComponent<Rigidbody2D>();
                }
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
        elasticBand.transform.position = StartLocation - BeaitifulRange;
        elasticBand.transform.rotation = startBandRotation;
        elasticBand.transform.localScale = startBandScale;
        elasticBand.GetComponent<SpriteRenderer>().flipX = false;
        elasticBand.GetComponent<SpriteRenderer>().flipY = false;
    }
    private static void ChangeBand(Vector3 mouseCoordinate , GameObject band , Vector2 startLocation)
	{
		var range = new Vector2(mouseCoordinate.x, mouseCoordinate.y) - startLocation;
		band.transform.position = new Vector3(mouseCoordinate.x, mouseCoordinate.y, 0);
		
        band.transform. = startLocation;
		var angle = Mathf.Atan2(range.y, range.x) * Mathf.Rad2Deg;
        band.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        band.GetComponent<SpriteRenderer>().flipX = true;
        if(Math.Abs(angle) >= 90 && Math.Abs(angle) < 180)
		{
            band.GetComponent<SpriteRenderer>().flipY = true;
        }
        else
		{
            band.GetComponent<SpriteRenderer>().flipY = false;
        }
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
