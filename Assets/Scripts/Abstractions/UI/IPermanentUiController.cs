using UI;


namespace Infrastructure {

	internal interface IPermanentUiController {
		void Init(ICoroutineRunner coroutineRunner, PermanentUiView permanentUiView);
		void ShowLoadingCurtain(bool showFading = true);
		void HideLoadingCurtain(bool showFading = true);
	}

}