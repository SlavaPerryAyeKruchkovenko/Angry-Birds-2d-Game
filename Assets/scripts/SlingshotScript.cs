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
        var game = Camera.main.GetComponent<GameScript>();
        if (!game.GameStart)
		{
            this.GetComponent<LineRenderer>().enabled = true;
			game.ChangeGameConditional();

            var position = new Vector3(this.transform.position.x, this.transform.position.y, -1);
            game.StartLocation = position;
            game.ChangeBird();
        }
		else
		{
            if(game.SelectedBird == null)
			{
                game.ChangeBird();
			}
		}
	}
}
