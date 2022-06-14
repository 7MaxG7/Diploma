namespace Infrastructure {

	internal interface ILobbyStatusDisplayer {
		bool IsLoading { get; set; }
		void ShowLoadingStatusAsync();
	}

}