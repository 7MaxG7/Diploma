using UnityEngine;
using Zenject;
using Random = System.Random;


namespace Infrastructure {

	class RandomController : IRandomController {
		private readonly Random _random = new();
		private readonly int _seed;
		
		[Inject]
		public RandomController() {
			if (_seed == 0) {
				_seed = _random.Next();
				Debug.Log($"Random seed = {_seed}");
			}

			_random = new Random(_seed);
		}

		public int GetRandomExcludingMax(int max = int.MaxValue, int min = 0) {
			return _random.Next(min, max);
		}

		public int GetRandomIncludingMax(int max = int.MaxValue, int min = 0) {
			if (max < int.MaxValue)
				max++;
			return GetRandomExcludingMax(max, min);
		}
	}

}