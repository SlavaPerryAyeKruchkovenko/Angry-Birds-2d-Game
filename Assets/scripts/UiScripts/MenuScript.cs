using Assets.scripts.ViewModel;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
	public MenuViewModel ViewModel { get; } = new MenuViewModel();
	[SerializeField]
	private GameObject settingsMenu;
	[SerializeField]
	private GameObject messageBox;
	private void Awake()
	{
		ViewModel.SerealizeUser(GameViewModel.GetUser());
		if (messageBox && ViewModel.User == null)
		{
			messageBox.SetActive(true);
			CloseMenu();
		}
	}
	public void OpenSettingsMenu()
	{
		settingsMenu.SetActive(true);
	}
	public void ExitGame()
	{
		CloseMenu();
		ViewModel.SaveUser();
		Application.Quit();
	}
	public void StartGame()
	{
		CloseMenu();
		ViewModel.SaveUser();
		StartCoroutine(GameViewModel.AsyncLoadNextLevel());
	}
	private void CloseMenu()
	{
		ViewModel.InvokeChangeConditionalUI(false);
		GameViewModel.SaveUser(ViewModel.User);
	}
}
