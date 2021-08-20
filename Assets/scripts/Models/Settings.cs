using System;

namespace Assets.scripts.Models
{
	public enum QualityImage
	{
		low, medium, high
	}
	public sealed class Settings<T> where T : Enum
	{
		public bool AimVisible = true;
		public float SoundValue = 1;
		public T Quality;
	}
}
