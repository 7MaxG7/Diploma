using Infrastructure;
using UI;
using Units.Views;
using UnityEngine;


namespace Abstractions.Services {

	internal interface IViewsFactory {
		GameObject CreateGameObject(string name);
		SoundPlayerView CreateSoundPlayer();
		MainMenuView CreateMainMenu();
		MissionUiView CreateMissionUi();
		void DestroyView(GameObject go);
	}

}