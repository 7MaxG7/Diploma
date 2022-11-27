using System.Threading.Tasks;
using Infrastructure;
using Sounds;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace Services
{
    internal interface IViewsFactory
    {
        GameObject CreateGameObject(string name);
        Task<GameObject> CreateGameObjectAsync(AssetReference prefab, Vector2 position, Quaternion rotation, Transform parent = null);
        SoundPlayerView CreateSoundPlayer();
        Task<MainMenuView> CreateMainMenuAsync();
        Task<LobbyCachedRoomItemView> CreateLobbyCachedRoomItemAsync(Transform parent = null);
        Task<RoomPlayerItemView> CreateRoomCachedPlayerItemAsync(Transform parent = null);
        Task<MissionUiView> CreateMissionUi();
        Task<SkillUiItemView> CreateSkillUiItemAsync(Transform parent = null);
    }
}