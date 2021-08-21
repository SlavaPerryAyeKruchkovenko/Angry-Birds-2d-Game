using Assets.scripts;
using Assets.scripts.Converters;
using Assets.scripts.UIModels;
using Assets.scripts.ViewModel;
using UnityEngine;
using UnityEngine.UI;

public class GameMessageBoxScript : MonoBehaviour
{
	MessageBox messageBox;
	private void Awake()
	{
		var text = gameObject.GetComponent<Text>();
		messageBox = new MessageBox(new Drawer(text, gameObject));
	}
	public void OpenMenu()
	{
		StartCoroutine(GameViewModel.AsyncLoadMenu());
	}
	public void NextLevel()
	{
		StartCoroutine(GameViewModel.AsyncLoadNextLevel());
	}
	private void OnMouseDown()
	{
		messageBox.BackConditional();
	}
}
