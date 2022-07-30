namespace Infrastructure {

	internal interface ISoundController {
		float MusicVolume { get; set; }
		float SoundVolume { get; set; }

		void Init();
		void PlayWeaponShootSound(WeaponType weaponType);
		void PlayRandomMenuMusic();
		void StopAll();
		void PlayRandomMissionMusic();
	}

}