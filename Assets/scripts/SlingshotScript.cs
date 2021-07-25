using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	private void OnMouseDown()
	{
        if(!Camera.main.GetComponent<GameScript>().GameStart)
		{
            this.GetComponent<LineRenderer>().enabled = true;
			Camera.main.GetComponent<GameScript>().ChangeGameConditional();

            var position = new Vector3(this.transform.position.x, this.transform.position.y, -1);
            Camera.main.GetComponent<GameScript>().StartLocation = position;
        }        
	}
}
