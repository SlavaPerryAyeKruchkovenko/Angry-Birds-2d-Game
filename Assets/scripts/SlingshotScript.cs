using System;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotScript : MonoBehaviour
{
	private readonly Queue<GameObject> birds = new Queue<GameObject>();
	private IObserver<GameObject> game;
	private IObserver<GameObject> cameraObserver;
	private void Awake()
	{
		game = Camera.main.GetComponent<GameScript>();
		cameraObserver = Camera.main.GetComponent<MainCameraScript>();
		var birds = GameObject.FindGameObjectsWithTag("Bird");
		foreach (var item in birds)
		{
			this.birds.Enqueue(item);
		}
	}
	private void AddBird()
	{
		if (birds.Count > 0)
		{
			var bird = birds.Dequeue();
			game.OnNext(bird);
			cameraObserver.OnNext(bird);
		}
	}
	private void OnMouseDown()
	{
		var game = Camera.main.GetComponent<GameScript>();
		if (!game.IsGameStart)
		{
			gameObject.GetComponent<LineRenderer>().enabled = true;
			game.InvokeStartGame();
			AddBird();
		}
		else if(!Camera.main.GetComponent<MainCameraScript>().NeedCheck)
		{
			AddBird();
		}
	}
}
