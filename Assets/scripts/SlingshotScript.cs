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
            var band = GameObject.Find("/Slingshot/elastic band");
            band.GetComponent<SpriteRenderer>().enabled = true;
			Camera.main.GetComponent<GameScript>().ChangeGameConditional();

            var position = new Vector2(band.transform.position.x, band.transform.position.y);
            Camera.main.GetComponent<GameScript>().StartLocation = (position + GameScript.BeaitifulRange);
        }        
	}
}
