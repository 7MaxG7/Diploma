using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;


namespace Units {

	internal class Experience {
		public event Action<int> OnExpChange;
		public event Action<int> OnLevelUp;

		public int CurrentExp {
			get => _currentExp;
			private set {
				_currentExp = value;
				OnExpChange?.Invoke(_currentExp);
			}
		}
		public int CurrentLevel {
			get => _currentLevel;
			private set {
				_currentLevel = value;
				OnLevelUp?.Invoke(_currentLevel);
			}
		}

		private readonly Dictionary<int,int> _levelParameters;
		private int _currentExp;
		private int _currentLevel;


		public Experience(int level, PlayerConfig.LevelExperienceParam[] levelParameters) {
			CurrentLevel = level;
			if (levelParameters != null)
				_levelParameters = levelParameters.ToDictionary(item => item.Level, item => item.TargetExp);
		}

		public void AddExp(int deltaExp) {
			CurrentExp += deltaExp;
			while (CurrentLevel < _levelParameters.Keys.Max() && CurrentExp >= _levelParameters[CurrentLevel + 1]) {
				CurrentLevel++;
			}
		}

		public int GetExpTarget(int level = -1) {
			return _levelParameters[level == -1 ? CurrentLevel + 1 : level];
		}
	}

}