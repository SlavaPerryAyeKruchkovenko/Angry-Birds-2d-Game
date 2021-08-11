using Assets.scripts;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    private GameObject _slingshot;
    private Quaternion _startRotation = default;
    private bool _isPress;

    public GameObject SelectedBird { get; private set; }
    public Bird Bird { get; private set; }
    public Queue<GameObject> Birds { get; private set; } = new Queue<GameObject>();
    public bool GameStart { get; private set; } = false;
    public Vector3 StartLocation { get; private set; } = default;

    // Start is called before the first frame update
    void Awake()
    {
        if (_slingshot != null) return;
        _slingshot = GameObject.Find("Slingshot");
        var position = new Vector3(_slingshot.transform.position.x, _slingshot.transform.position.y, -1);
        StartLocation = position;
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
            if (!CompareVectors3(mouseCoor, cameraLeftButtomCoor) || !CompareVectors3(cameraRightUpCoor, mouseCoor))
            {
                _isPress = false;
                Bird.InvokeResetEvent();
            }
            else if (Input.GetMouseButtonDown(0))
            {
                _isPress = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _isPress = false;
                ResetBand();
                if (!SelectedBird.GetComponent<Rigidbody2D>() && Mathf.Abs(StartLocation.x - mouseCoor.x) > 1)
                {
                    var mainCameraScript = Camera.main.GetComponent<MainCameraScript>();
                    SelectedBird.AddComponent<Rigidbody2D>();
                    Bird.InvokeFlyEvent(StartLocation - mouseCoor);
                    if (mainCameraScript)
                    {
                        mainCameraScript.AddBird(SelectedBird);
                        mainCameraScript.LockCamera = false;
                    }
                    GameStart = false;
                    //Make Fly Animation;
                }
                else
                {
                    ResetBird();
                }
            }
            else if (_isPress)
            {
                Camera.main.GetComponent<MainCameraScript>().LockCamera = true;
                Bird.InvokeTakeAimEvent(mouseCoor);
                var script = SelectedBird.GetComponent<BirdScript>();
                script.DrawTraectory(SelectedBird.transform.right * (StartLocation - mouseCoor).sqrMagnitude);
            }
        }
    }
    private void DropBird(Vector3 range)//when Bird Start Flying
    {
        var power = Vector3.SqrMagnitude(range) / 2 * 0.8f;//delta x^2 * k /2
        SelectedBird.GetComponent<Rigidbody2D>().AddForce(SelectedBird.transform.right * power, ForceMode2D.Impulse);
    }
    private void ChangeBand(Vector3 mouseCoordinate)
    {
        var rangeVector = new Vector3(StartLocation.x - 0.3f, StartLocation.y + 0.1f, -1);
        _slingshot.GetComponent<LineRenderer>().enabled = true;
        _slingshot.GetComponent<LineRenderer>().SetPositions(new Vector3[] { StartLocation, rangeVector, mouseCoordinate });
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
            Bird.ResetBird += ResetBand;
            Bird.ReadyFly += DropBird;
            _startRotation = SelectedBird.transform.rotation;
        }
    }
    private static float GetAngle(Vector3 vector) => Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
    public static bool CompareVectors3(Vector3 vector1, Vector3 vector2)//if 1 postion will be more that true
    {
        if (vector1.x > vector2.x && vector1.y > vector2.y)
            return true;
        return false;
    }
    private void ResetBird()//ResetPosition
    {
        SelectedBird.transform.position = StartLocation;
        SelectedBird.transform.rotation = _startRotation;
    }
    private void ResetBand()
    {
        _slingshot.GetComponent<LineRenderer>().enabled = false;
    }
    public void ChangeGameConditional() => this.GameStart = !this.GameStart;
}

