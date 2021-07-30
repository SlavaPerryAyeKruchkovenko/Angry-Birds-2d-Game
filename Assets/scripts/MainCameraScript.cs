using Assets.scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainCameraScript : MonoBehaviour
{
    public GameObject FlyingBird { get; private set; }
    private Vector3 startPosition;
    private GameObject background;
    private Vector3 lastLocation;
    private Vector3? range = null;
    private float firstClickTime = 0;
    private int clickCount = 0;
    public bool LockCamera = false;
    // Start is called before the first frame update
    void Start()
    {
        background = GameObject.FindGameObjectWithTag("Background");
        startPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!LockCamera)
		{
            lastLocation = this.transform.position;
            if (clickCount >= 2)
            {
                ResetCamera();
                clickCount = 0;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                firstClickTime = Time.time;
                if(range == null)
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
                if (Time.time - firstClickTime < 0.1f)
                {
                    clickCount++;
                }
                else
                {
                    clickCount = 0;
                }
                
                if(range != null)
				{
                    range -= Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    this.gameObject.transform.position += new Vector3(range.Value.x, range.Value.y, 0);
                }               
                if (!CanMoveCamera(background, Camera.main))
                {
                    this.gameObject.transform.position = lastLocation;
                }
                range = null;
            }
            else if (FlyingBird != null)
            {
                var coor = FlyingBird.transform.position;
                this.gameObject.transform.position = new Vector3(coor.x, coor.y, transform.position.z);
                if (!CanMoveCamera(background, Camera.main))
                {
                    this.gameObject.transform.position = lastLocation;
                }
            }
        }
        else
		{
            range = null;
		}
    }
    public void AddBird(GameObject bird)
	{
        if(bird.GetComponent<GameObjectScript>() && bird.GetComponent<GameObjectScript>().ABGameObj is Bird)
		{
            this.FlyingBird = bird;
            this.FlyingBird.GetComponent<GameObjectScript>().ABGameObj.BirdDie += ResetCamera;
		}
	}
    public void ResetCamera()
	{
        this.gameObject.transform.position = startPosition;
	}
    private static bool CanMoveCamera(GameObject background, Camera camera)
	{
        var rect = background.GetComponent<RectTransform>();
        float width = background.transform.localScale.x * rect.rect.width; // weight
        float height = background.transform.localScale.y * rect.rect.height;// height
        Vector2 coor2 = background.transform.position;
        float lX = coor2.x - width / 2; //left x coor
        float rX = coor2.x + width / 2; //right x coor
        float upY = coor2.y + height / 2;// up X coor
        float buttomY = coor2.y - height / 2;// buttom Y coor
        var cameraLeftButtomCoor = camera.ViewportToWorldPoint(new Vector2(0, 0));
        var cameraRightUpCoor = camera.ViewportToWorldPoint(new Vector2(1, 1));
        return lX < cameraLeftButtomCoor.x &&
            buttomY < cameraLeftButtomCoor.y &&
            rX > cameraRightUpCoor.x &&
            upY > cameraRightUpCoor.y;
    }
}
