using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Infrastructure {

	[CreateAssetMenu(menuName = "Configs/" + nameof(PlayerConfig), fileName = nameof(PlayerConfig), order = 4)]
	internal class PlayerConfig : ScriptableObject {
		[Serializable]
		internal class LevelExperienceParam {
			[SerializeField] private int _level;
			[SerializeField] private int _targetExp;
			
			public int Level => _level;
			public int TargetExp => _targetExp;
		}
		
		
		[SerializeField] private float _baseMoveSpeed;
		[SerializeField] private int _baseHp;
		[SerializeField] private LevelExperienceParam[] _levelParameters;

		public float BaseMoveSpeed => _baseMoveSpeed;
		public int BaseHp => _baseHp;
		public LevelExperienceParam[] LevelParameters => _levelParameters;
	}

}