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
	private MessageBox MessageBox;
	private Animator animator;
	// Start is called before the first frame update
	void Awake()
	{
		animator = GetComponent<Animator>();
		MessageBox = new MessageBox(gameObject, new Drawer(errorText,gameObject));
		viewModel = transform.parent.GetComponent<MenuScript>().ViewModel;
		viewModel.InvalidClick += () => MessageBox.ShowError(true);
	}

	// Update is called once per frame
	public void CheckOnError(string name)
	{
		HideButton(false);
		if (MessageBox.CheckSyntax(name))
			PrintError();
		errorText.enabled = false;
	}
	public void UpdateName(string name)
	{
		HideButton(true);
		MessageBox.ChangeText(name);
	}
	private void PrintError()
	{
		if (errorText)
		{
			MessageBox.PrintSyntaxError();
			errorText.enabled = true;
		}
	}
	private void ClearText()
	{
		if (text)
			text.text = string.Empty;
	}
	public void CloseMessageBox()
	{
		ClearText();
		viewModel.SerealizeUser(new User(MessageBox.text.ToString()));
		viewModel.InvokeChangeConditionalUI(true);
		this.gameObject.SetActive(false);
	}
	private void HideButton(bool value)
	{
		if (button)
			button.SetActive(value);
	}
	
	private void OnMouseDown()
	{
		MessageBox.ShowError(false);
	}
}
