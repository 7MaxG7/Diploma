using TMPro;
using UnityEngine;


namespace UI {

	internal interface IPermanentUiView {
		GameObject GameObject { get; }
		Canvas PermanentCanvas { get; }
		CanvasGroup LoadingCurtainCanvasGroup { get; }
		TMP_Text LoadingCurtainText { get; }
		SettingsPanelView SettingsPanel { get; }
		GameObject MissionSettingsPanel { get; }
	}

}