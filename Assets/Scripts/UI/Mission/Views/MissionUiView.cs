using System;
using Infrastructure;
using UnityEngine;
using UnityEngine.UI;


namespace UI
{
    internal sealed class MissionUiView : MonoBehaviour
    {
        [SerializeField] private PlayerPanelView _playerPanel;
        [SerializeField] private SkillsPanelView _skillsPanel;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private CanvasGroup _compassPointerCanvasGroup;

        public event Action OnSettingsClick;

        public PlayerPanelView PlayerPanel => _playerPanel;
        public SkillsPanelView SkillsPanel => _skillsPanel;

        private UiConfig _uiConfig;


        public void Init(UiConfig uiConfig)
        {
            _uiConfig = uiConfig;
            _settingsButton.onClick.AddListener(() => OnSettingsClick?.Invoke());
            _compassPointerCanvasGroup.alpha = 0;
            _compassPointerCanvasGroup.gameObject.SetActive(false);
        }

        public void OnDispose()
        {
            _settingsButton.onClick.RemoveAllListeners();
        }

        public void ShowCompass(Vector3 closestEnemyPlayerDestination)
        {
            _compassPointerCanvasGroup.gameObject.SetActive(true);
            if (_compassPointerCanvasGroup.alpha < 1)
                _compassPointerCanvasGroup.alpha += _uiConfig.ArrowPointerFadingFrameDelta;
            _compassPointerCanvasGroup.transform.up = closestEnemyPlayerDestination;
        }

        public void HideCompass(Vector3 closestEnemyPlayerDestination)
        {
            if (!_compassPointerCanvasGroup.gameObject.activeSelf)
                return;

            if (_compassPointerCanvasGroup.alpha > 0)
            {
                _compassPointerCanvasGroup.alpha -= _uiConfig.ArrowPointerFadingFrameDelta;
                _compassPointerCanvasGroup.transform.up = closestEnemyPlayerDestination;
            }
            else
                _compassPointerCanvasGroup.gameObject.SetActive(false);
        }
    }
}