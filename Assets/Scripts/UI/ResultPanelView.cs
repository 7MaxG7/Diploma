using System;
using DG.Tweening;
using Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace UI {

	internal sealed class ResultPanelView : MonoBehaviour {
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private TMP_Text _resultLableText;
		[SerializeField] private TMP_Text _killsAmount;
		[SerializeField] private Button _closePanelButton;

		public event Action OnCloseResultPanelClick;
		
		private UiConfig _uiConfig;
		

		public void Init(UiConfig uiConfig) {
			_uiConfig = uiConfig;
			_closePanelButton.onClick.AddListener(() => OnCloseResultPanelClick?.Invoke());
		}

		public void ShowMissionResult(bool isWinner, int killsAmount, float animationDuration) {
			gameObject.SetActive(true);
			_resultLableText.text = isWinner ? _uiConfig.WinResultText : _uiConfig.LooseResultText;
			_resultLableText.color = isWinner ? _uiConfig.WinResultColor : _uiConfig.LooseResultColor;
			_killsAmount.text = killsAmount.ToString();
			_canvasGroup.DOKill();
			_canvasGroup.alpha = 0;
			_canvasGroup.DOFade(1, animationDuration);
		}

		public void HideMissionResult() {
			_canvasGroup.DOKill();
			gameObject.SetActive(false);
		}

		public void Dispose() {
			_closePanelButton.onClick.RemoveAllListeners();
		}
	}

}