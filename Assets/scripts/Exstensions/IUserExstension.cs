using Assets.scripts.Models;

namespace Assets.scripts.Exstensions
{
	public static class IUserExstension
	{
		public static bool IsEmpty(this IUser user)
		{
			return user == null;
		}
	}
}
