using System.Threading.Tasks;
using Infrastructure;
using Sounds;
using UI;
using UnityEngine;


namespace Services
{
    internal interface IViewsFactory
    {
        GameObject CreateGameObject(string name);
        GameObject CreatePhotonObj(string playerConfigPlayerPrefabPath, Vector2 position, Quaternion rotation);
        SoundPlayerView CreateSoundPlayer();
        Task<MainMenuView> CreateMainMenuAsync();
        Task<LobbyCachedRoomItemView> CreateLobbyCachedRoomItemAsync(Transform parent = null);
        Task<RoomPlayerItemView> CreateRoomCachedPlayerItemAsync(Transform parent = null);
        Task<MissionUiView> CreateMissionUi();
        Task<SkillUiItemView> CreateSkillUiItemAsync(Transform parent = null);
    }
}