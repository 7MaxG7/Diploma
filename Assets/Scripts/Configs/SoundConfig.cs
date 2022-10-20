using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Weapons;


namespace Sounds
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(SoundConfig), fileName = nameof(SoundConfig), order = 7)]
    internal sealed class SoundConfig : ScriptableObject
    {
        [SerializeField] private SoundPlayerView _soundPlayerPrefab;
        [SerializeField] private WeaponShootingClip[] _weaponShootingClips;
        [SerializeField] private AudioClip[] _menuMusic;
        [SerializeField] private AudioClip[] _missionMusic;

        public SoundPlayerView SoundPlayerPrefab => _soundPlayerPrefab;
        public AudioClip[] MenuMusic => _menuMusic;
        public AudioClip[] MissionMusic => _missionMusic;


        public Dictionary<WeaponType, AudioClip> GetWeaponShootingClips()
        {
            return _weaponShootingClips.ToDictionary(clip => clip.WeaponType, clip => clip.AudioClip);
        }
    }
}