using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.scripts.Models
{
	public enum QualityImage
	{
		low,medium, high
	}
	public class Setting
	{
		public bool AimVisible = true;
		public float SoundValue = 1;
		public QualityImage quality = QualityImage.high;
	}
}
