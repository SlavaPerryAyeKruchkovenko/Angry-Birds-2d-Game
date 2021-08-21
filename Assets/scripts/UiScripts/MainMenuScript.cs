using Assets.scripts;
using Assets.scripts.ViewModel;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
	// Start is called before the first frame update
	private readonly List<GameObject> child = new List<GameObject>();
	private MenuViewModel viewModel;
	private void Awake()
	{
		viewModel = transform.parent.GetComponent<MenuScript>().ViewModel;
		viewModel.ChangeConditionalUI += TurnChild;

		for (int i = 0; i < transform.childCount; i++)
			child.Add(transform.GetChild(i).gameObject);		
	}
	public void TurnChild(bool value)
	{
		foreach (var item in child)
			item.SetActive(value);
	}
}
