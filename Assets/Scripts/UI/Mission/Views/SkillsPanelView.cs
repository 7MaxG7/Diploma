using System;
using System.Collections.Generic;
using DG.Tweening;
using Infrastructure;
using UnityEngine;


namespace UI {

	internal sealed class SkillsPanelView : MonoBehaviour {
		[SerializeField] private Transform _skillsUpgradeItemsContent;
		[SerializeField] private CanvasGroup _canvasGroup;

		public event Action<ActualSkillInfo> OnSkillChoosen;

		private UiConfig _uiConfig;
		private readonly List<SkillUiItemView> _skillItems = new();

		
		public void Init(UiConfig uiConfig) {
			_uiConfig = uiConfig;
			_canvasGroup.alpha = 0;
		}

		public void OnDispose() {
			foreach (var skillItem in _skillItems) {
				skillItem.OnSkillChoosen -= InvokeSkillChoosen;
				skillItem.OnDispose();
			}
			_skillItems.Clear();
			_canvasGroup.DOKill();
		}

		public void ShowSkills(ActualSkillInfo[] skills) {
			while (_skillItems.Count < skills.Length) {
				var skillItem = Instantiate(_uiConfig.SkillUiItemPrefab, _skillsUpgradeItemsContent);
				skillItem.Init(_uiConfig);
				skillItem.OnSkillChoosen += InvokeSkillChoosen;
				_skillItems.Add(skillItem);
			}
			for (var i = 0; i < _skillItems.Count; i++) {
				_skillItems[i].gameObject.SetActive(i < skills.Length);
			}

			for (var i = 0; i < skills.Length; i++) {
				_skillItems[i].Setup(skills[i]);
			}
			ToggleSkillsPanelVisibility(true);
		}

		private void InvokeSkillChoosen(ActualSkillInfo skill) {
			OnSkillChoosen?.Invoke(skill);
			ToggleSkillsPanelVisibility(false);
		}

		private void ToggleSkillsPanelVisibility(bool isVisible) {
			_canvasGroup.DOKill();
			if (isVisible) {
				_canvasGroup.gameObject.SetActive(true);
				_canvasGroup.DOFade(1, _uiConfig.CanvasFadeAnimationDuration)
						.SetDelay(_uiConfig.SkillChooserActivationDelay);
			} else {
				_canvasGroup.DOFade(0, _uiConfig.CanvasFadeAnimationDuration)
						.OnComplete(() => _canvasGroup.gameObject.SetActive(false));
			}
		}
	}

}