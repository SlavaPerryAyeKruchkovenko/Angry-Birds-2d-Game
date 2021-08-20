using System;
using System.Collections.Generic;
using UnityEngine;

internal class SlingshotScript : MonoBehaviour
{
	private readonly Queue<GameObject> birds = new Queue<GameObject>();
	private IObserver<GameObject> game;
	private IObserver<GameObject> cameraObserver;
	private LineRenderer lineRenderer;
	private GameScript gameScript;
	private MainCameraScript mainCameraScript;
	private void Awake()
	{
		mainCameraScript = Camera.main.GetComponent<MainCameraScript>();
		gameScript = Camera.main.GetComponent<GameScript>();
		lineRenderer = GetComponent<LineRenderer>();
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
		if (!gameScript.IsGameStart)
		{
			lineRenderer.enabled = true;
			gameScript.InvokeStartGame();
			AddBird();
		}
		else if (!mainCameraScript.NeedCheck)
		{
			AddBird();
		}
	}
}
