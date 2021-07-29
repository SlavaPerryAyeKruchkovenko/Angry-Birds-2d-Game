using Assets.scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    public GameObject FlyingBird { get; private set; }
    private Vector3 startPosition;
    private GameObject background;
    private Vector3 lastLocation;
    // Start is called before the first frame update
    void Start()
    {       
        background = GameObject.FindGameObjectWithTag("Background");
        startPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        lastLocation = this.transform.position;
        if (FlyingBird != null)
		{
            var coor = FlyingBird.transform.position;
            this.gameObject.transform.position = new Vector3(coor.x, coor.y, transform.position.z);
            if (!CanMoveCamera(background, Camera.main))
			{
                this.gameObject.transform.position = lastLocation;              
            }
        }
        else
		{
            ResetCamera();
		}
    }
    public void AddBird(GameObject bird)
	{
        if(bird.GetComponent<GameObjectScript>() && bird.GetComponent<GameObjectScript>().ABGameObj is Bird)
		{
            this.FlyingBird = bird;
		}
	}
    private void ResetCamera()
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
