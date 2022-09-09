using System;


namespace Infrastructure {

	internal interface IPermanentUiController {
		event Action OnCurtainShown;
		event  Action OnLeaveGameClicked;
		event  Action OnResultPanelClosed;
		bool IsActivating { get; }
		bool CurtainIsActive { get; }

		void Init(ICoroutineRunner coroutineRunner);
		void ShowLoadingCurtain(bool isAnimated = true);
		void HideLoadingCurtain(bool animationIsOn = true, bool interruptCurrentAnimation = false);
		void ShowSettingsPanel(bool missionSettingsSectionIsActive = false);
		void ShowMissionResult(MissionEndInfo missionEndInfo);
	}

}