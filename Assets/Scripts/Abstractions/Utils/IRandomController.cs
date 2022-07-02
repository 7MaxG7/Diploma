namespace Infrastructure {

	internal interface IRandomController {
		int GetRandomExcludingMax(int max = int.MaxValue, int min = 0);
		int GetRandomIncludingMax(int max = int.MaxValue, int min = 0);
	}

}