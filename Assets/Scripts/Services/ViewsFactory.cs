using Infrastructure;
using Photon.Pun;
using Sounds;
using UI;
using UnityEngine;
using Utils;
using Zenject;
using static UnityEngine.Object;


namespace Services {

	internal sealed class ViewsFactory : IViewsFactory {
		private readonly IPhotonManager _photonManager;
		private readonly SoundConfig _soundConfig;
		private readonly MainMenuConfig _mainMenuConfig;
		private readonly UiConfig _uiConfig;


		[Inject]
		public ViewsFactory(IPhotonManager photonManager, SoundConfig soundConfig, MainMenuConfig mainMenuConfig, UiConfig uiConfig) {
			_photonManager = photonManager;
			_soundConfig = soundConfig;
			_mainMenuConfig = mainMenuConfig;
			_uiConfig = uiConfig;
		}

		public GameObject CreateGameObject(string name) {
			return new GameObject(name);
		}

		public GameObject CreatePhotonObj(string prefabPath, Vector2 position, Quaternion rotation) {
			return _photonManager.Create(prefabPath, position, rotation);
		}

		public SoundPlayerView CreateSoundPlayer() {
			return Instantiate(_soundConfig.SoundPlayerPrefab);
		}

		public MainMenuView CreateMainMenu() {
			return Instantiate(_mainMenuConfig.MainMenuPref);
		}

		public MissionUiView CreateMissionUi() {
			var uiRoot = GameObject.Find(Constants.UI_ROOT_NAME) ?? new GameObject(Constants.UI_ROOT_NAME);
			return Instantiate(_uiConfig.MissionUiView, uiRoot.transform);
		}

		public void DestroyView(MonoBehaviour view) {
			Destroy(view.gameObject);
		}

		public void DestroyPhotonObj(PhotonView obj) {
			_photonManager.Destroy(obj);
		}
	}

}