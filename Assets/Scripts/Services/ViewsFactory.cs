using Abstractions.Services;
using Infrastructure;
using UI;
using Units.Views;
using UnityEngine;
using Utils;
using Zenject;
using static UnityEngine.Object;


namespace Services {

	internal class ViewsFactory : IViewsFactory {
		private readonly SoundConfig _soundConfig;
		private readonly MainMenuConfig _mainMenuConfig;
		private readonly UiConfig _uiConfig;


		[Inject]
		public ViewsFactory(SoundConfig soundConfig, MainMenuConfig mainMenuConfig, UiConfig uiConfig) {
			_soundConfig = soundConfig;
			_mainMenuConfig = mainMenuConfig;
			_uiConfig = uiConfig;
		}

		public GameObject CreateGameObject(string name) {
			return new GameObject(name);
		}

		public SoundPlayerView CreateSoundPlayer() {
			return Instantiate(_soundConfig.SoundPlayerPrefab);
		}

		public MainMenuView CreateMainMenu() {
			return Instantiate(_mainMenuConfig.MainMenuPref);
		}

		public MissionUiView CreateMissionUi() {
			var uiRoot = GameObject.Find(TextConstants.UI_ROOT_NAME) ?? new GameObject(TextConstants.UI_ROOT_NAME);
			return Instantiate(_uiConfig.MissionUiView, uiRoot.transform);
		}

		public void DestroyView(GameObject go) {
			Destroy(go);
		}
	}

}