using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Infrastructure{

	internal interface IMainMenuView {
		GameObject GameObject { get; }
		TMP_Text HeaderLabel { get; }
		TMP_Text LoginButtonText { get; }
		Button LoginPanelButton { get; }
		Button PlayButton { get; }
		Button QuitGameButton { get; }
		// Panels
		LoginPanelView LoginPanelView { get; }
		LobbyScreenView LobbyScreenView { get; }
	}

}