using Infrastructure;
using Photon.Pun;
using Sounds;
using UI;
using UnityEngine;


namespace Services {

	internal interface IViewsFactory {
		GameObject CreateGameObject(string name);
		GameObject CreatePhotonObj(string playerConfigPlayerPrefabPath, Vector2 position, Quaternion rotation);
		SoundPlayerView CreateSoundPlayer();
		MainMenuView CreateMainMenu();
		MissionUiView CreateMissionUi();
	}

}