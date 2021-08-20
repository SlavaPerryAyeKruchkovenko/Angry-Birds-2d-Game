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
		if (messageBox)
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
	private void CloseMenu()
	{
		ViewModel.InvokeChangeConditionalUI(false);
	}
}
