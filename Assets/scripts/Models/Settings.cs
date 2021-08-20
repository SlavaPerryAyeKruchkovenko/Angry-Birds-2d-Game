using System;

namespace Assets.scripts.Models
{
	public enum QualityImage
	{
		low, medium, high
	}
	public sealed class Settings<T> where T : Enum
	{
		public bool AimVisible;
		public float SoundValue;
		public T Quality;
	}
}
