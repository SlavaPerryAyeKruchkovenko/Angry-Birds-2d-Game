using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public GameObject SelectedBird { get; set; }
    public Queue<GameObject> Birds = new Queue<GameObject>();
    public bool GameStart { get; private set;} = false;
    public Vector2 StartLocation = default;
    // Start is called before the first frame update
    void Start()
    {
        SelectedBird = Birds.Dequeue();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cour = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (GameStart)
        {
            if(!Input.GetMouseButtonDown(0))
			{
                SelectedBird.transform.position = StartLocation;
			}
        }

    }
    public void ChangeGameConditional() => this.GameStart = !this.GameStart;
}
