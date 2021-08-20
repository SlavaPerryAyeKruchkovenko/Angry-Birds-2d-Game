using Assets.scripts.Models;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Assets.scripts.ViewModel
{
	internal static class GameViewModel
	{
		public static event Action GameEnd;
		private static readonly Game game = new Game();
		public static void Awake()
		{
			GameEnd += game.SaveUser;
		}
		public static IEnumerator AsyncLoadNextLevel()
		{			
			var operation = SceneManager.LoadSceneAsync(game.User.LevelNow);
			if (!operation.isDone)
				yield return null;
		}
		public static IEnumerator AsyncLoadMenu()
		{
			var operation = SceneManager.LoadSceneAsync(0);
			if (!operation.isDone)
				yield return null;
		}
		public static void LevelComplete()
		{
			game.WinGame(GameEnd);
		}
		public static IUser GetUser()
		{
			return game.User;
		}
		public static void SetUser(IUser user)
		{
			game.UploadUser(user);
		}
	}
}
