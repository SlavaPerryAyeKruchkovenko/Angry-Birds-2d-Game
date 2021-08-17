using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
