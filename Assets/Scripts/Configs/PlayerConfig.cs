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
		
		[Serializable]
		internal class LevelHealthParam {
			[SerializeField] private int _level;
			[SerializeField] private int _health;

			public int Level => _level;
			public int Health => _health;
		}

		
		[SerializeField] private string _playerPrefabPath;
		[SerializeField] private float _baseMoveSpeed;
		[SerializeField] private LevelExperienceParam[] _levelExpParameters;
		[SerializeField] private LevelHealthParam[] _levelHpParameters;

		public float BaseMoveSpeed => _baseMoveSpeed;
		public LevelExperienceParam[] LevelExpParameters => _levelExpParameters.OrderBy(item => item.Level).ToArray();
		public LevelHealthParam[] LevelHpParameters => _levelHpParameters.OrderBy(item => item.Level).ToArray();
		public string PlayerPrefabPath => _playerPrefabPath;
	}

}