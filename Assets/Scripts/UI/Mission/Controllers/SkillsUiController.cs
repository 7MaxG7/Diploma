using System;
using System.Collections.Generic;
using DG.Tweening;
using Infrastructure;
using Utils;
using Object = UnityEngine.Object;


namespace UI {

	internal class SkillsUiController : IDisposable {
		public event Action<WeaponType> OnSkillChoose;
		
		private readonly UiConfig _uiConfig;
		private readonly SkillsPanelView _skillsPanel;
		private readonly SkillUiItemView _skillUiItemPrefab;
		
		private readonly List<SkillUiItemView> _skillItems = new();


		public SkillsUiController(SkillsPanelView skillsPanel, UiConfig uiConfig) {
			_skillsPanel = skillsPanel;
			_uiConfig = uiConfig;
			_skillUiItemPrefab = _uiConfig.SkillUiItemPrefab;
		}

		public void Dispose() {
			foreach (var skillItem in _skillItems) {
				skillItem.Button.onClick.RemoveAllListeners();
			}
			_skillItems.Clear();
			_skillsPanel.CanvasGroup.DOKill();
		}

		public void ShowSkillsChoose(ActualSkillInfo[] skills) {
			var deltaAmount = skills.Length - _skillItems.Count;
			if (deltaAmount > 0) {
				for (var i = 0; i < deltaAmount; i++) {
					_skillItems.Add(Object.Instantiate(_skillUiItemPrefab, _skillsPanel.SkillsUpgradeItemsContent));
				}
			} else if (deltaAmount < 0) {
				for (var i = 0; i < Math.Abs(deltaAmount); i++) {
					var skillItemIndex = _skillItems.Count - 1 - i;
					_skillItems[skillItemIndex].GameObject.SetActive(false);
				}
			}

			for (var i = 0; i < skills.Length; i++) {
				UpdateChoosingSkillData(skills[i], _skillItems[i]);
			}
			ToggleSkillsPanelVisibility(true);
		}

		private void UpdateChoosingSkillData(ActualSkillInfo skill, SkillUiItemView skillItem) {
			skillItem.Button.onClick.RemoveAllListeners();
			skillItem.Button.onClick.AddListener(() => UpgradeSkill(skill));
			skillItem.SkillNameLable.text = skill.SkillName;
			skillItem.SkillLevelLable.text = skill.Level == 1 ? TextConstants.NEW_WEAPON_LEVEL_TEXT : string.Format(TextConstants.UPGRADE_WEAPON_LEVEL_TEXT, skill.Level);
			skillItem.SkillDescription.text = skill.SkillDescription;
		}

		private void UpgradeSkill(ActualSkillInfo skill) {
			OnSkillChoose?.Invoke(skill.WeaponType);
			ToggleSkillsPanelVisibility(false);
		}

		private void ToggleSkillsPanelVisibility(bool isVisible) {
			_skillsPanel.CanvasGroup.DOKill();
			if (isVisible) {
				_skillsPanel.CanvasGroup.gameObject.SetActive(true);
				_skillsPanel.CanvasGroup.DOFade(1, _uiConfig.CanvasFadeAnimationDuration).SetDelay(_uiConfig.SkillChooserActivationDelay);
			} else {
				_skillsPanel.CanvasGroup.DOFade(0, _uiConfig.CanvasFadeAnimationDuration)
						.OnComplete(() => {
							_skillsPanel.CanvasGroup.gameObject.SetActive(false);
						});

			}
		}

	}

}