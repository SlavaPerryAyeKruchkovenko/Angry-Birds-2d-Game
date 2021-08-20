using Assets.scripts;
using Assets.scripts.ViewModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMessageBoxScript : MonoBehaviour
{
	public void OpenMenu()
	{
		StartCoroutine(GameViewModel.AsyncLoadMenu());
	}
	public void NextLevel()
	{
		StartCoroutine(GameViewModel.AsyncLoadNextLevel());
	}
}
