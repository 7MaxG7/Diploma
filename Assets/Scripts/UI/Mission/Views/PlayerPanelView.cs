using Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace UI
{
    internal sealed class PlayerPanelView : MonoBehaviour
    {
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private Slider _expSlider;
        [SerializeField] private TMP_Text _levelText;

        private UiConfig _uiConfig;


        public void Init(UiConfig uiConfig)
        {
            _uiConfig = uiConfig;
        }

        public void UpdateHealth(float currentHp, float maxHp)
        {
            _healthSlider.value = currentHp / maxHp;
            _healthText.text = string.Format(_uiConfig.HealthBarTextTemplate, (int)currentHp, (int)maxHp);
        }

        public void UpdateLevel(int level)
        {
            _levelText.text = string.Format(_uiConfig.ExperienceBarLevelTextTemplate, level);
        }

        public void UpdateExperience(int currentExp, int currentLevelExpMin, int currentLevelExpMax)
        {
            _expSlider.value = (float)(currentExp - currentLevelExpMin) / (currentLevelExpMax - currentLevelExpMin);
        }
    }
}