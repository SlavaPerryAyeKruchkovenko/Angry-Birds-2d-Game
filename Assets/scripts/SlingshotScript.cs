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
            Camera.main.GetComponent<GameScript>().ChangeGameConditional();
            Camera.main.GetComponent<GameScript>().StartLocation = this.gameObject.transform.position;
        }        
	}
}
