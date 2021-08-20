using Assets.scripts.Models;
using Assets.scripts.ViewModel;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingScript : MonoBehaviour
{
	private Settings<QualityImage> setting;
	private MenuViewModel viewModel;
	private void Awake()
	{
		viewModel = transform.parent.GetComponent<MenuScript>().ViewModel;
		var user = viewModel.User as User;
		setting = user.Setting;
		if (setting == null)
			setting = new Settings<QualityImage>();

		AwakeStartSettings();
	}
	public void ChangeSettings(bool value)
	{
		setting.AimVisible = value;
	}
	public void ChangeSettings(float value)
	{
		setting.SoundValue = value;
	}
	public void ChangeSettings(int value)
	{
		int count = Enum.GetNames(typeof(QualityImage)).Length;
		if (value > 0 && value < count)
			setting.Quality = (QualityImage)Enum.GetValues(typeof(QualityImage)).GetValue(value);
		else
			Debug.Log("incorrect value");
	}
	public void ChangeSettings(QualityImage value)
	{
		setting.Quality = value;
	}
	public void CloseSettingMenu()
	{
		viewModel.ChangeSettings(setting);
		viewModel.SaveUser();
		gameObject.SetActive(false);
	}
	public void LoadSettingMenu()
	{
		gameObject.SetActive(true);
		viewModel.LoadUser();
		AwakeStartSettings();
	}
	public void AwakeStartSettings()
	{
		var toggle = GameObject.Find("Toggle");
		toggle.GetComponent<Toggle>().isOn = setting.AimVisible;
		toggle.GetComponent<ToggleButtonScript>().Awake();
		GameObject.Find("Slider").GetComponent<Slider>().value = setting.SoundValue;
		GameObject.Find("Dropdown").GetComponent<Dropdown>().value = (int)setting.Quality;
	}
}
