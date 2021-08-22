using Assets.scripts.Converters;
using Assets.scripts.Models;
using Assets.scripts.UIModels;
using Assets.scripts.ViewModel;
using UnityEngine;
using UnityEngine.UI;

public class MessageBoxScript : MonoBehaviour
{
	[SerializeField]
	private Text errorText;
	[SerializeField]
	private Text text;
	[SerializeField]
	private GameObject button;

	private MenuViewModel viewModel;
	private MessageBox messageBox;
	// Start is called before the first frame update
	void Awake()
	{
		messageBox = new MessageBox(new Drawer(errorText, gameObject));
		viewModel = transform.parent.GetComponent<MenuScript>().ViewModel;
	}
	public void CheckOnError(string name)
	{
		HideButton(false);
		if (messageBox.CheckSyntax(name))
			PrintError();
		errorText.enabled = false;
	}
	public void UpdateName(string name)
	{
		HideButton(true);
		messageBox.ChangeText(name);
	}
	private void PrintError()
	{
		if (errorText)
		{
			messageBox.PrintSyntaxError();
			errorText.enabled = true;
		}
	}
	public void CloseMessageBox()
	{
		messageBox.ClearText();
		viewModel.SerealizeUser(new User(messageBox.Text.ToString()));
		viewModel.InvokeChangeConditionalUI(true);
		GameViewModel.SaveUser(viewModel.User);
		viewModel.ChangeProperty();
		this.gameObject.SetActive(false);
	}
	private void HideButton(bool value)
	{
		if (button)
			button.SetActive(value);
	}
}
