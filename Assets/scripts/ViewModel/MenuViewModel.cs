using Assets.scripts.Exstensions;
using Assets.scripts.Models;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Assets.scripts.ViewModel
{
	public class MenuViewModel
	{
		public event Action PropertyChangedEvent;
		public event Action<bool> ChangeConditionalUI;
		public IUser User { get; private set; }
		private void CreateUser(IUser user)
		{
			if (user is User newUser && !user.IsEmpty())
				User = newUser;
			else
				return;
		}
		public void SerealizeUser(IUser user)
		{
			CreateUser(user);
			LoadUser();
		}		
		public void LoadUser()
		{
			if(User != null)
			{
				try
				{
					User.Load();
				}
				catch (Exception)
				{
					User.Save();
				}
			}			
		}
		public void DeserealizeUser(IUser user)
		{
			CreateUser(user);
			SaveUser();
		}
		public void SaveUser()
		{
			User.Save();
		}
		public void ChangeSettings(Settings<QualityImage> settings)
		{
			var user = User as User;
			user.ChangeProperty(settings);
		}
		public void InvokeChangeConditionalUI(bool isSee)
		{
			if (ChangeConditionalUI != null)
				ChangeConditionalUI.Invoke(isSee);
		}
		public void ChangeProperty()
		{
			PropertyChangedEvent.Invoke();
		}
	}
}
