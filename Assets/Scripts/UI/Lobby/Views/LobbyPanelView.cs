using System;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Realtime;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Infrastructure
{
    internal sealed class LobbyPanelView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Transform _roomsListContent;
        [SerializeField] private TMP_InputField _privateRoomNameInputText;
        [SerializeField] private Button _createPrivateRoomButton;
        [SerializeField] private Button _joinPrivateRoomButton;
        [SerializeField] private Button _createNewRoomButton;
        [SerializeField] private Button _joinRandomRoomButton;
        [SerializeField] private Button _closePanelButton;

        public event Action<string> OnCreatePrivateRoomClick;
        public event Action<string> OnJoinPrivateRoomClick;
        public event Action<string> OnJoinRoomClick;
        public event Action OnCreateNewRoomClick;
        public event Action OnJoinRandomRoomClick;
        public event Action OnClosePanelClick;

        private readonly Dictionary<string, LobbyCachedRoomItemView> _cachedRoomItemViews = new();
        private bool _uiIsBlocked;
        private MainMenuConfig _mainMenuConfig;
        private IViewsFactory _viewsFactory;


        public void Init(IViewsFactory viewsFactory, MainMenuConfig mainMenuConfig)
        {
            _viewsFactory = viewsFactory;
            _mainMenuConfig = mainMenuConfig;
            _createPrivateRoomButton.onClick.AddListener(InvokePrivateRoomCreation);
            _joinPrivateRoomButton.onClick.AddListener(InvokePrivateRoomJoin);
            _createNewRoomButton.onClick.AddListener(InvokeNewRoomCreation);
            _joinRandomRoomButton.onClick.AddListener(InvokeRandomRoomJoin);
            _closePanelButton.onClick.AddListener(InvokePanelClose);
            _privateRoomNameInputText.onValueChanged.AddListener(UpdatePrivateRoomButtonsInteractivity);

            void InvokePrivateRoomCreation()
            {
                ToggleBlockingUi(true);
                OnCreatePrivateRoomClick?.Invoke(_privateRoomNameInputText.text);
            }

            void InvokePrivateRoomJoin()
            {
                ToggleBlockingUi(true);
                OnJoinPrivateRoomClick?.Invoke(_privateRoomNameInputText.text);
            }

            void InvokeNewRoomCreation()
            {
                ToggleBlockingUi(true);
                OnCreateNewRoomClick?.Invoke();
            }

            void InvokeRandomRoomJoin()
            {
                ToggleBlockingUi(true);
                OnJoinRandomRoomClick?.Invoke();
            }

            void InvokePanelClose()
            {
                ToggleBlockingUi(true);
                OnClosePanelClick?.Invoke();
            }
        }

        public void Show(Action onPanelShownCallback)
        {
            ClearPanel();
            ToggleBlockingUi(true);
            gameObject.SetActive(true);
            _canvasGroup.alpha = 0;
            _canvasGroup.DOFade(1, _mainMenuConfig.LobbyPanelFadingDuration)
                .OnComplete(() => onPanelShownCallback?.Invoke());
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

        public async void AddRoom(RoomInfo room)
        {
            var roomName = room.Name;
            if (!_cachedRoomItemViews.ContainsKey(roomName))
            {
                var roomItem = await _viewsFactory.CreateLobbyCachedRoomItemAsync(_roomsListContent);
                roomItem.RoomName.text = roomName;
                roomItem.RoomButton.onClick.AddListener(() => OnJoinRoomClick?.Invoke(roomName));
                roomItem.RoomName.color = room.IsOpen
                    ? _mainMenuConfig.UnlockedRoomLabelColor
                    : _mainMenuConfig.LockedRoomLabelColor;
                _cachedRoomItemViews[roomName] = roomItem;
            }
        }

        public void RemoveRoom(RoomInfo room)
        {
            var roomName = room.Name;
            if (_cachedRoomItemViews.ContainsKey(roomName))
            {
                _cachedRoomItemViews[roomName].RoomButton.onClick.RemoveAllListeners();
                Destroy(_cachedRoomItemViews[roomName].gameObject);
                _cachedRoomItemViews.Remove(roomName);
            }
        }

        public void UnblockUi()
        {
            ToggleBlockingUi(false);
        }

        public void OnDispose()
        {
            ClearPanel();
            _createPrivateRoomButton.onClick.RemoveAllListeners();
            _joinPrivateRoomButton.onClick.RemoveAllListeners();
            _createNewRoomButton.onClick.RemoveAllListeners();
            _joinRandomRoomButton.onClick.RemoveAllListeners();
            _closePanelButton.onClick.RemoveAllListeners();
            _privateRoomNameInputText.onValueChanged.RemoveAllListeners();
        }

        private void ClearPanel()
        {
            foreach (var roomItemView in _cachedRoomItemViews.Values)
            {
                roomItemView.RoomButton.onClick.RemoveAllListeners();
                Destroy(roomItemView.gameObject);
            }

            _cachedRoomItemViews.Clear();
        }

        private void ToggleBlockingUi(bool isBlocked)
        {
            _uiIsBlocked = isBlocked;

            _createNewRoomButton.interactable = !isBlocked;
            _joinRandomRoomButton.interactable = !isBlocked;
            _closePanelButton.interactable = !isBlocked;
            UpdatePrivateRoomButtonsInteractivity(_privateRoomNameInputText.text);
            foreach (var roomItem in _cachedRoomItemViews.Values)
            {
                roomItem.RoomButton.interactable = !isBlocked;
            }
        }

        private void UpdatePrivateRoomButtonsInteractivity(string privateRoomName)
        {
            _createPrivateRoomButton.interactable = !_uiIsBlocked && !string.IsNullOrEmpty(privateRoomName) &&
                                                    !_cachedRoomItemViews.ContainsKey(privateRoomName);
            _joinPrivateRoomButton.interactable = !_uiIsBlocked && !string.IsNullOrEmpty(privateRoomName);
        }
    }
}