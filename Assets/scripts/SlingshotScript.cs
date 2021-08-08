using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotScript : MonoBehaviour
{
	private void OnMouseDown()
	{
        var game = Camera.main.GetComponent<GameScript>();
        if (!game.GameStart)
		{
            this.GetComponent<LineRenderer>().enabled = true;
			game.ChangeGameConditional();
          
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
