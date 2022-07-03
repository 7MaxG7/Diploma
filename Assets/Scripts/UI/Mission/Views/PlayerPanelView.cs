using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace UI {

	internal class PlayerPanelView : MonoBehaviour {
		[SerializeField] private Slider _healthSlider;
		[SerializeField] private TMP_Text _healthText;
		[SerializeField] private Slider _expSlider;
		[SerializeField] private TMP_Text _levelText;

		public Slider HealthSlider => _healthSlider;
		public TMP_Text HealthText => _healthText;
		public Slider ExpSlider => _expSlider;
		public TMP_Text LevelText => _levelText;
	}

}