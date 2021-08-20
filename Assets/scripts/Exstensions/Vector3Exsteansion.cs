using UnityEngine;

namespace Assets.scripts.Exstensions
{
	public static class Vector3Exsteansion
	{
		public static System.Numerics.Vector3 ConvertUnityVectorInBase(this Vector3 vector)
		{
			return new System.Numerics.Vector3(vector.x, vector.y, vector.z);
		}
		public static Vector3 ConvertBaseVectorInUnity(this System.Numerics.Vector3 vector)
		{
			return new Vector3(vector.X, vector.Y, vector.Z);
		}
	}
}
