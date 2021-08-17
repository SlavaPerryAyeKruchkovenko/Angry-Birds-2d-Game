using Assets.scripts.Models;
using UnityEngine;
using UnityEngine.UI;

public class SettingScript : MonoBehaviour
{
	private User user;
	private Setting setting;
	private void Awake()
	{
		if(user != null)
		{
			AwakeStartSettings(user.Name);
			setting = user.Setting;
		}
		else
		{
			setting = new Setting();
		}
	}
	public void ChangeSettings(bool value)
	{
		setting.AimVisible = value;
	}
	public void ChangeSettings(float value)
	{
		setting.SoundValue = value;
	}
	public void CloseSettingMenu()
	{
		if(user != null)
		{
			user.ChangeProperty(setting);
			user.Save();
		}
			
		this.gameObject.SetActive(false);
	}
	public void LoadSettingMenu(string name)
	{
		user = new User(name);
		user.Load();
		this.gameObject.SetActive(true);
		AwakeStartSettings(name);
	}
	private void AwakeStartSettings(string name)
	{
		if (user == null)
			user = new User(name);
		GameObject.Find("Toggle").GetComponent<Toggle>().isOn = user.Setting.AimVisible;
	}	
}
