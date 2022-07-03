using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;


namespace Units {

	internal class Experience {
		public event Action<int> OnExpChange;

		public int CurrentExp {
			get => _currentExp;
			private set {
				_currentExp = value;
				OnExpChange?.Invoke(_currentExp);
			}
		}
		public int CurrentLevel { get; private set; }

		private readonly Dictionary<int,int> _levelParameters;
		private int _currentExp;


		public Experience(int level, PlayerConfig.LevelExperienceParam[] levelParameters) {
			CurrentLevel = level;
			if (levelParameters != null)
				_levelParameters = levelParameters.ToDictionary(item => item.Level, item => item.TargetExp);
		}

		public void AddExp(int deltaExp) {
			CurrentExp += deltaExp;
			while (CurrentExp > _levelParameters[CurrentLevel]) {
				CurrentLevel++;
			}
		}

		public int GetExpTarget(int level = -1) {
			return _levelParameters[level == -1 ? CurrentLevel + 1 : level];
		}
	}

}