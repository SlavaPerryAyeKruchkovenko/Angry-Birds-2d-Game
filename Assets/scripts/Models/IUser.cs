namespace Assets.scripts.Models
{
	public interface IUser
	{
		string Name { get; }
		int LevelsComplete { get; }
		int LevelNow { get; }
		void ChangeProperty(bool IsWin);
		void ChangeProperty(IUser user);
		void Save();
		void Load();
	}
}
