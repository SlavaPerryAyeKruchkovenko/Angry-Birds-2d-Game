using Newtonsoft.Json;
using System;
using System.IO;


namespace Assets.scripts.Models
{
	public class User : IUser
	{
		public User(string name)
		{
			Name = name;
			Setting = new Settings<QualityImage>();
		}

		public string Name { get; private set; }
		[JsonProperty]
		public Settings<QualityImage> Setting { get; private set; }
		[JsonProperty]
		public int LevelsComplete { get; private set; }		
		public int LevelNow => LevelsComplete + 1;
		private string Path => Environment.CurrentDirectory + "\\Assets\\Saves\\" + Name + ".json";

		public int DropBird;
		public int KillPig;
		public void ChangeProperty(IUser user)
		{
			Name = user.Name;
			LevelsComplete = user.LevelsComplete;
		}
		public void ChangeProperty(User user)
		{
			Name = user.Name;
			Setting = user.Setting;
			DropBird = user.DropBird;
			KillPig = user.KillPig;
			LevelsComplete = user.LevelsComplete;
		}
		public void ChangeProperty(Settings<QualityImage> setting)
		{
			Setting = setting;
		}
		public void Save()
		{
			var content = JsonConvert.SerializeObject(this);
			if (!File.Exists(Path))
				File.Create(Path).Dispose();
			File.WriteAllText(Path, content);
		}
		public void ChangeProperty(bool isWin)
		{
			LevelsComplete += isWin ? 1 : 0;
		}
		public void Load()
		{
			if (!File.Exists(Path))
				throw new Exception("FileNotFound");
			string json = File.ReadAllText(Path);
			var user = JsonConvert.DeserializeObject<User>(json,new JsonSerializerSettings()
				{
					TypeNameHandling = TypeNameHandling.All
				});
			ChangeProperty(user ?? throw new Exception("Empty File"));
		}		
	}
}
