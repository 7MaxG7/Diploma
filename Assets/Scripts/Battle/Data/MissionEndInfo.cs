namespace Infrastructure {

	internal sealed class MissionEndInfo {
		public bool IsWinner { get; }
		public int KillsAmount { get; }

		public MissionEndInfo(bool isWinner, int killsAmount) {
			IsWinner = isWinner;
			KillsAmount = killsAmount;
		}
	}

}