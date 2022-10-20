using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Infrastructure
{
    internal sealed class RoomPanelView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Transform _playersListContent;
        [SerializeField] private TMP_Text _roomPanelHeader;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _closePanelButton;

        public event Action OnStartGameClick;
        public event Action OnClosePanelClick;

        private MainMenuConfig _mainMenuConfig;
        private readonly Dictionary<string, RoomPlayerItemView> _cachedPlayerItemViews = new();


        public void Init(MainMenuConfig mainMenuConfig)
        {
            _mainMenuConfig = mainMenuConfig;
            _startGameButton.onClick.AddListener(() => OnStartGameClick?.Invoke());
            _closePanelButton.onClick.AddListener(() => OnClosePanelClick?.Invoke());
        }

        public void OnDispose()
        {
            DOTween.KillAll();
            _startGameButton.onClick.RemoveAllListeners();
            _closePanelButton.onClick.RemoveAllListeners();
        }

        public void Show(string roomName, Action onPanelShownCallback)
        {
            ClearPanel();
            gameObject.SetActive(true);
            _canvasGroup.alpha = 0;
            _roomPanelHeader.text = roomName;
            ToggleBlockingUi(true);
            _canvasGroup.DOFade(1, _mainMenuConfig.LobbyPanelFadingDuration)
                .OnComplete(() =>
                {
                    onPanelShownCallback?.Invoke();
                    ToggleBlockingUi(false);
                });
        }

        public void Hide(bool isAnimated = true, Action onPanelHiddenCallback = null)
        {
            if (isAnimated)
            {
                ToggleBlockingUi(true);
                _canvasGroup.DOFade(0, _mainMenuConfig.LobbyPanelFadingDuration)
                    .OnComplete(FinishHiding);
            }
            else
            {
                FinishHiding();
            }

            void FinishHiding()
            {
                gameObject.SetActive(false);
                ClearPanel();
                onPanelHiddenCallback?.Invoke();
            }
        }

        public void ToggleMasterButtons(bool isActive)
        {
            _startGameButton.gameObject.SetActive(isActive);
        }

        public void AddPlayerItem(string playerName)
        {
            if (!_cachedPlayerItemViews.ContainsKey(playerName))
            {
                var playerItem = Instantiate(_mainMenuConfig.RoomCachedPlayerItemPref, _playersListContent);
                playerItem.PlayerName.text = playerName;
                _cachedPlayerItemViews.Add(playerName, playerItem);
            }
        }

        public void RemovePlayerItem(string playerName)
        {
            if (_cachedPlayerItemViews.ContainsKey(playerName))
            {
                Destroy(_cachedPlayerItemViews[playerName].gameObject);
                _cachedPlayerItemViews.Remove(playerName);
            }
        }

        public void BlockUi()
        {
            ToggleBlockingUi(true);
        }

        private void ToggleBlockingUi(bool mustBlocked)
        {
            _startGameButton.interactable = !mustBlocked;
            _closePanelButton.interactable = !mustBlocked;
        }

        private void ClearPanel()
        {
            foreach (var playerItem in _cachedPlayerItemViews.Values)
            {
                Destroy(playerItem.gameObject);
            }

            _cachedPlayerItemViews.Clear();
        }
    }
}