using UnityEngine;


namespace Units.Views {

	internal sealed class SoundPlayerView : MonoBehaviour {
		[SerializeField] private AudioSource _musicSource;
		[SerializeField] private AudioSource _sfxSource;
		
		public AudioSource MusicSource => _musicSource;
		public AudioSource SfxSource => _sfxSource;
	}

}