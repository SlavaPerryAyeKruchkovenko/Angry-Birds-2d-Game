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
			Setting = new Setting();
		}
		public string Name { get; private set; }
		public Setting Setting { get; private set; }
		public int DropBird;
		public int KillPig;
		public void ChangeProperty(IUser user)
		{
			Name = user.Name;
		}
		public void ChangeProperty(User user)
		{
			Name = user.Name;
			Setting = user.Setting;
			DropBird = user.DropBird;
			KillPig = user.KillPig;
		}
		public void ChangeProperty(Setting setting)
		{
			Setting = setting;
		}
		public void Save()
		{
			var path = Environment.CurrentDirectory + Name + ".json";
			var content = JsonConvert.SerializeObject(this);
			if (!File.Exists(path))
				File.Create(path).Dispose();
			File.WriteAllText(path, content);
		}
		public void Load()
		{
			var path = Environment.CurrentDirectory + Name + ".json";
			if (!File.Exists(path))
				return;
			string json = File.ReadAllText(path);
			var user = JsonConvert.DeserializeObject<User>(json);
			ChangeProperty(user ?? throw new Exception("Empty File"));
		}
	}
}
