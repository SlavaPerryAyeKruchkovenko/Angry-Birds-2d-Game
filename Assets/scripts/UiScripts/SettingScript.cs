using Assets.scripts.Models;
using UnityEngine;
using UnityEngine.UI;

public class SettingScript : MonoBehaviour
{
	public Setting setting;
	private void Awake()
	{
		if (setting == null)
			setting = new Setting();
		GameObject.Find("Toggle").GetComponent<Toggle>().isOn = setting.AimVisible;
	}
	public void ChangeSettings(bool value)
	{
		setting.AimVisible = value;
	}
	public void ChangeSettings(float value)
	{
		setting.SoundValue = value;
	}
}
