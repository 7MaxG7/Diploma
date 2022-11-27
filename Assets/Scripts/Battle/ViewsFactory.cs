using System.Threading.Tasks;
using Infrastructure;
using Sounds;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils;
using Zenject;
using static UnityEngine.Object;


namespace Services
{
    internal sealed class ViewsFactory : IViewsFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly SoundConfig _soundConfig;
        private readonly MainMenuConfig _mainMenuConfig;
        private readonly UiConfig _uiConfig;
        private IPunEventRaiser _punEventRaiser;


        [Inject]
        public ViewsFactory(IAssetProvider assetProvider, SoundConfig soundConfig, MainMenuConfig mainMenuConfig
            , UiConfig uiConfig)
        {
            _assetProvider = assetProvider;
            _soundConfig = soundConfig;
            _mainMenuConfig = mainMenuConfig;
            _uiConfig = uiConfig;
        }

        public GameObject CreateGameObject(string name)
        {
            return new GameObject(name);
        }

        public async Task<GameObject> CreateGameObjectAsync(AssetReference assetReference, Vector2 position,
            Quaternion rotation, Transform parent = null)
        {
            var prefab = await _assetProvider.LoadAsync<GameObject>(assetReference);
            return Instantiate(prefab, position, rotation, parent);
        }

        public SoundPlayerView CreateSoundPlayer()
        {
            return Instantiate(_soundConfig.SoundPlayerPrefab);
        }

        public async Task<MainMenuView> CreateMainMenuAsync()
        {
            return await CreateInstanceAsync<MainMenuView>(_mainMenuConfig.MainMenuPref);
        }

        public async Task<LobbyCachedRoomItemView> CreateLobbyCachedRoomItemAsync(Transform parent = null)
        {
            return await CreateInstanceAsync<LobbyCachedRoomItemView>(_mainMenuConfig.LobbyCachedRoomItemPref, parent);
        }

        public async Task<RoomPlayerItemView> CreateRoomCachedPlayerItemAsync(Transform parent = null)
        {
            return await CreateInstanceAsync<RoomPlayerItemView>(_mainMenuConfig.RoomCachedPlayerItemPref, parent);
        }

        public async Task<MissionUiView> CreateMissionUi()
        {
            var uiRoot = GameObject.Find(Constants.UI_ROOT_NAME) ?? new GameObject(Constants.UI_ROOT_NAME);
            return await CreateInstanceAsync<MissionUiView>(_uiConfig.MissionUiView, uiRoot.transform);
        }

        public async Task<SkillUiItemView> CreateSkillUiItemAsync(Transform parent = null)
        {
            return await CreateInstanceAsync<SkillUiItemView>(_uiConfig.SkillUiItemPrefab, parent);
        }

        private async Task<T> CreateInstanceAsync<T>(AssetReference assetReference, Transform parent = null) where T : MonoBehaviour
        {
            var prefab = await _assetProvider.LoadAsync<GameObject>(assetReference);
            return Instantiate(prefab.GetComponent<T>(), parent);
        }
    }
}