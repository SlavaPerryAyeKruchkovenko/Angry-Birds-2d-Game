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
		public bool AimVisible;
		public float SoundValue { get=> SoundValue; set
			{
				if (value <= 1) SoundValue = value;
				else throw new Exception("Uncorrect Value");
			} 
		}
		public QualityImage quality = QualityImage.high;
	}
}
