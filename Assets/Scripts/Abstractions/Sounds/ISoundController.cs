namespace Infrastructure {

	internal interface ISoundController {
		void Init();
		float GetMusicVolume();
		void SetMusicVolume(float volume);
		float GetSoundVolume();
		void SetSoundVolume(float obj);
		void PlayWeaponShootSound(WeaponType weaponType);
		void PlayRandomMenuMusic();
		void StopAll();
		void PlayRandomMissionMusic();

	}

}