using System;


namespace Infrastructure {

	internal interface IPermanentUiController {
		event Action OnCurtainShown;
		bool IsActivating { get; }
		bool IsActive { get; }

		void Init(ICoroutineRunner coroutineRunner);
		void ShowLoadingCurtain(bool animationIsOn = true, bool isForced = false);
		void HideLoadingCurtain(bool animationIsOn = true, bool isForced = false);
		void ShowSettingsPanel(bool missionPanelIsActive = false);
	}

}