using System;
using UnityEngine;


namespace Infrastructure {

	[Serializable]
	internal class LevelExperienceParam {
		[SerializeField] private int _level;
		[SerializeField] private int _targetExp;
			
		public int Level => _level;
		public int TargetExp => _targetExp;
	}

}