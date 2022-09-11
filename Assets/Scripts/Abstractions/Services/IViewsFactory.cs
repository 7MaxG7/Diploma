using Infrastructure;
using Photon.Pun;
using UI;
using Units.Views;
using UnityEngine;


namespace Services {

	internal interface IViewsFactory {
		GameObject CreateGameObject(string name);
		GameObject CreatePhotonObj(string playerConfigPlayerPrefabPath, Vector2 position, Quaternion rotation);
		SoundPlayerView CreateSoundPlayer();
		MainMenuView CreateMainMenu();
		MissionUiView CreateMissionUi();
		void DestroyView(MonoBehaviour view);
		void DestroyPhotonObj(PhotonView obj);
	}

}