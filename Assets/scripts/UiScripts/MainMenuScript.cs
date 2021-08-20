using Assets.scripts.ViewModel;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
	// Start is called before the first frame update
	private List<GameObject> child = new List<GameObject>();
	private MenuViewModel viewModel;
	private void Awake()
	{
		viewModel = transform.parent.GetComponent<MenuScript>().ViewModel;
		for (int i = 0; i < transform.childCount; i++)
		{
			child.Add(transform.GetChild(i).gameObject);
		}
		viewModel.ChangeConditionalUI += TurnChild;
	}
	public void TurnChild(bool value)
	{
		foreach (var item in child)
		{
			item.SetActive(value);
		}
	}
	private void OnMouseDown()
	{
		viewModel.SelectWindow();
	}
}
