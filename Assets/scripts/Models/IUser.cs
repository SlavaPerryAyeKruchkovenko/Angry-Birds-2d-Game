namespace Assets.scripts.Models
{
	public interface IUser
	{
		string Name { get; }
		void ChangeProperty(IUser user);
		void Save();
		void Load();
	}
}
