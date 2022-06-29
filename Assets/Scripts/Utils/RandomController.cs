using UnityEngine;
using Zenject;
using Random = System.Random;


namespace Infrastructure {

	class RandomController : IRandomController {
		private readonly Random _random = new();
		private readonly int seed;
		
		[Inject]
		public RandomController() {
			if (seed == 0) {
				seed = _random.Next();
				Debug.Log($"Random seed = {seed}");
			}

			_random = new Random(seed);
		}

		public int GetRandom(int max = int.MaxValue, int min = 0) {
			return _random.Next(min, max);
		}
	}

}