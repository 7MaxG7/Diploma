using System;
using Infrastructure;
using UnityEngine;


namespace UI {

	internal interface IPermanentUiView {
		event Action OnCurtainShown;
		
		GameObject GameObject { get; }
		bool CurtainIsActive { get; }
		bool CurtainIsActivating { get; }
		SettingsPanelView SettingsPanel { get; }
		ResultPanelView ResultPanel { get; }

		void Init(ICoroutineRunner coroutineRunner, UiConfig uiConfig);
		void ShowLoadingCurtain(bool isAnimated);
		void HideLoadingCurtain(bool animationIsOn);
		void ShowLoadingStatusLableAsync();
		void StopLoadingCurtainAnimation();
		void Dispose();
	}

}