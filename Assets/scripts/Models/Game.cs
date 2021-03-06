using Assets.scripts.Models;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.scripts
{
	public class Game
	{
		public IUser User { get; private set; }		
		public void WinGame(Action GameEnd)
		{
			if(User != null)
			{
				User.ChangeProperty(true);
				User.Save();
			}
			if (GameEnd != null)
			{
				GameEnd.Invoke();
			}				
		}
		public void SaveUser()
		{
			if (User != null)
				User.Save();
		}
		public void UploadUser(IUser user)
		{
			User = user;
		}
		public Settings<QualityImage> GetSettings()
		{
			var user = User as User;
			if (user != null)
				return user.Setting;
			else
				return new Settings<QualityImage>();
		}
	}
}
