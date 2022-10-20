using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using Services;
using UnityEngine;
using Weapons;
using Zenject;
using Object = UnityEngine.Object;


namespace Sounds
{
    internal sealed class SoundController : ISoundController
    {
        private readonly SoundConfig _soundConfig;
        private readonly IRandomManager _random;
        private readonly IViewsFactory _viewsFactory;
        private readonly IPlayerPrefsService _playerPrefsService;
        private SoundPlayerView _soundPlayer;
        private Dictionary<WeaponType, AudioClip> _weaponShootClips;
        private AudioClip[] _menuClips;
        private AudioClip[] _missionClips;


        [Inject]
        public SoundController(SoundConfig soundConfig, IRandomManager randomManager, IViewsFactory viewsFactory
            , IPlayerPrefsService playerPrefsService)
        {
            _soundConfig = soundConfig;
            _random = randomManager;
            _viewsFactory = viewsFactory;
            _playerPrefsService = playerPrefsService;
        }

        public void Init()
        {
            InitPlayer();
            InitClips();

            void InitPlayer()
            {
                if (_soundPlayer == null)
                    _soundPlayer = _viewsFactory.CreateSoundPlayer();
                _soundPlayer.MusicLoop = true;
                Object.DontDestroyOnLoad(_soundPlayer);
                var volumes = _playerPrefsService.GetSoundVolumes();
                if (volumes != null)
                {
                    SetMusicVolume(volumes.Value.MusicVolume);
                    SetSoundVolume(volumes.Value.SoundVolume);
                }
            }

            void InitClips()
            {
                _weaponShootClips = _soundConfig.GetWeaponShootingClips();
                _menuClips = _soundConfig.MenuMusic.Where(clip => clip != null).ToArray();
                _missionClips = _soundConfig.MissionMusic.Where(clip => clip != null).ToArray();
            }
        }

        public float GetMusicVolume()
        {
            return _soundPlayer.MusicVolume;
        }

        public void SetMusicVolume(float volume)
        {
            _soundPlayer.MusicVolume = volume;
        }

        public float GetSoundVolume()
        {
            return _soundPlayer.SoundVolume;
        }

        public void SetSoundVolume(float volume)
        {
            _soundPlayer.SoundVolume = volume;
        }

        public void PlayWeaponShootSound(WeaponType weaponType)
        {
            if (_weaponShootClips.ContainsKey(weaponType))
                _soundPlayer.PlaySound(_weaponShootClips[weaponType]);
        }

        public void PlayRandomMenuMusic()
        {
            if (_menuClips == null || _menuClips.Length == 0)
                return;

            _soundPlayer.StopMusic();
            var randomClipIndex = _random.GetRandomExcludingMax(_menuClips.Length);
            _soundPlayer.PlayMusic(_menuClips[randomClipIndex]);
        }

        public void PlayRandomMissionMusic()
        {
            if (_menuClips == null || _menuClips.Length == 0)
                return;

            _soundPlayer.StopMusic();
            var randomClipIndex = _random.GetRandomExcludingMax(_missionClips.Length);
            _soundPlayer.PlayMusic(_missionClips[randomClipIndex]);
        }


        public void StopAll()
        {
            _soundPlayer.StopMusic();
            _soundPlayer.StopSound();
        }
    }
}