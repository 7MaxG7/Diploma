using System;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;


namespace Infrastructure {

	internal sealed class LobbyScreenView : MonoBehaviour {
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private TMP_Text _statusLableText;
		[SerializeField] private LobbyPanelView _lobbyPanelView;
		[SerializeField] private RoomPanelView _roomPanelView;

		public LobbyPanelView LobbyPanelView => _lobbyPanelView;
		public RoomPanelView RoomPanelView => _roomPanelView;

		private MainMenuConfig _mainMenuConfig;
		private bool _loadingStatusIsOn;


		public void Init(MainMenuConfig mainMenuConfig) {
			_mainMenuConfig = mainMenuConfig;
		}

		public void Show() {
			_canvasGroup.DOKill();
			ShowLobbyScreenWithAnimation();
			ShowLoadingStatusAsync();


			void ShowLobbyScreenWithAnimation() {
				gameObject.SetActive(true);
				_canvasGroup.alpha = 0;
				_canvasGroup.DOFade(1, _mainMenuConfig.LobbyPanelFadingDuration);
			}

		}

		public void HideScreen(Action screenHiddenCallback) {
			_canvasGroup.DOKill();
			_canvasGroup.DOFade(0, _mainMenuConfig.LobbyPanelFadingDuration)
					.OnComplete(() => {
								gameObject.SetActive(false);
								screenHiddenCallback?.Invoke();
								_loadingStatusIsOn = false;
					});
		}

		public void StopLoadingStatus() {
			_loadingStatusIsOn = false;
		}

		public async void ShowLoadingStatusAsync() {
			_loadingStatusIsOn = true;
			_statusLableText.color = _mainMenuConfig.OrdinaryStatusTextColor;
			var currentAwaitingTick = _mainMenuConfig.MinStatusSuffixSymbolsAmount;
			while (_loadingStatusIsOn) {
				_statusLableText.text = $"{_mainMenuConfig.LoadingStatusPrefixText}{new string(_mainMenuConfig.LoadingStatusSuffixSymbol, currentAwaitingTick)}";
				if (++currentAwaitingTick >= _mainMenuConfig.MaxStatusSuffixSymbolsAmount)
					currentAwaitingTick = _mainMenuConfig.MinStatusSuffixSymbolsAmount;
				await Task.Delay(_mainMenuConfig.StatusLableSuffixUpdateDelay);
			}
			_statusLableText.text = string.Empty;
		}

	}

}