using System.Collections.Generic;
using System.Linq;
using Units.Views;
using UnityEngine;
using Utils;
using Zenject;


namespace Infrastructure {

	internal class SoundController : ISoundController {
		public float MusicVolume {
			get => _soundPlayer.MusicSource.volume;
			set => _soundPlayer.MusicSource.volume = value;
		}
		public float SoundVolume {
			get => _soundPlayer.SfxSource.volume;
			set => _soundPlayer.SfxSource.volume = value;
		}

		private readonly SoundConfig _soundConfig;
		private readonly IRandomController _random;
		private SoundPlayerView _soundPlayer;
		private Dictionary<WeaponType, AudioClip> _weaponShootClips;
		private AudioClip[] _menuClips;
		private AudioClip[] _missionClips;


		[Inject]
		public SoundController(SoundConfig soundConfig, IRandomController randomController) {
			_soundConfig = soundConfig;
			_random = randomController;
		}
		
		public void Init() {
			if (_soundPlayer == null)
				_soundPlayer = Object.Instantiate(_soundConfig.SoundPlayerPrefab);
			_soundPlayer.MusicSource.loop = true;
			_weaponShootClips = _soundConfig.GetWeaponShootingClips();
			_menuClips = _soundConfig.MenuMusic.Where(clip => clip != null).ToArray();
			_missionClips = _soundConfig.MissionMusic.Where(clip => clip != null).ToArray();
			if (PlayerPrefs.HasKey(TextConstants.MUSIC_VOLUME_PREFS_KEY)) {
				_soundPlayer.MusicSource.volume = PlayerPrefs.GetFloat(TextConstants.MUSIC_VOLUME_PREFS_KEY);
			}
			if (PlayerPrefs.HasKey(TextConstants.SOUND_VOLUME_PREFS_KEY)) {
				_soundPlayer.SfxSource.volume = PlayerPrefs.GetFloat(TextConstants.SOUND_VOLUME_PREFS_KEY);
			}
			Object.DontDestroyOnLoad(_soundPlayer);
		}

		public void PlayWeaponShootSound(WeaponType weaponType) {
			if (_weaponShootClips.ContainsKey(weaponType))
				_soundPlayer.SfxSource.PlayOneShot(_weaponShootClips[weaponType]);
		}

		public void PlayRandomMenuMusic() {
			if (_menuClips == null || _menuClips.Length == 0)
				return;

			_soundPlayer.MusicSource.Stop();
			var randomClipIndex = _random.GetRandomExcludingMax(_menuClips.Length);
			_soundPlayer.MusicSource.clip = _menuClips[randomClipIndex];
			_soundPlayer.MusicSource.Play();
		}

		public void PlayRandomMissionMusic() {
			if (_menuClips == null || _menuClips.Length == 0)
				return;

			_soundPlayer.MusicSource.Stop();
			var randomClipIndex = _random.GetRandomExcludingMax(_missionClips.Length);
			_soundPlayer.MusicSource.clip = _missionClips[randomClipIndex];
			_soundPlayer.MusicSource.Play();
		}

		public void StopAll() {
			_soundPlayer.MusicSource.Stop();
			_soundPlayer.SfxSource.Stop();
		}

	}

}