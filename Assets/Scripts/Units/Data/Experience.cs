using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using Unity.Mathematics;


namespace Units {

	internal class Experience {
		public event Action<int> OnExpChange;
		public event Action<int> OnLevelUp;

		public int CurrentExp {
			get => _currentExp;
			private set {
				var exp = Math.Min(value, MaxExp);
				if (exp != _currentExp) {
					_currentExp = exp;
					OnExpChange?.Invoke(_currentExp);
				}
			}
		}
		
		public int CurrentLevel {
			get => _currentLevel;
			private set {
				var level = Math.Min(value, _maxLevel);
				if (level != _currentExp) {
					_currentLevel = level;
					OnLevelUp?.Invoke(_currentLevel);
				}
			}
		}

		public int MaxExp { get; }
		
		private readonly Dictionary<int,int> _levelParameters;
		private int _currentExp;
		private int _currentLevel;
		private readonly int _maxLevel;


		public Experience(int level, PlayerConfig.LevelExperienceParam[] levelParameters) {
			if (levelParameters != null) {
				_levelParameters = levelParameters.ToDictionary(item => item.Level, item => item.TargetExp);
				_maxLevel = _levelParameters.Keys.Max();
				MaxExp = _levelParameters.Values.Max();
			} else {
				_maxLevel = level;
			}
			CurrentLevel = level;
		}

		public void AddExp(int deltaExp) {
			CurrentExp += deltaExp;
			while (CurrentLevel < _maxLevel && CurrentExp >= _levelParameters[CurrentLevel + 1]) {
				CurrentLevel++;
			}
		}

		public int GetExpTarget(int level = -1) {
			if (_levelParameters == null) {
				return -1;
			}
			level = Math.Min(level == -1 ? CurrentLevel + 1 : level, _maxLevel);
			return _levelParameters[level];
		}
	}

}