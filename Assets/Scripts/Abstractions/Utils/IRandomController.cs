namespace Infrastructure {

	internal interface IRandomController {
		int GetRandom(int max = int.MaxValue, int min = 0);
	}

}