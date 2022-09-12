using System;
using Infrastructure;
using TMPro;
using Units;
using UnityEngine;
using UnityEngine.UI;


namespace UI {

	internal sealed class SkillUiItemView : MonoBehaviour {
		[SerializeField] private Button _button;
		[SerializeField] private TMP_Text _skillNameLable;
		[SerializeField] private TMP_Text _skillLevelLable;
		[SerializeField] private TMP_Text _skillDescription;

		public event Action<ActualSkillInfo> OnSkillChoosen;
		
		private UiConfig _uiConfig;

		
		public void Init(UiConfig uiConfig) {
			_uiConfig = uiConfig;
		}

		public void OnDispose() {
			_button.onClick.RemoveAllListeners();
		}

		public void Setup(ActualSkillInfo skill) {
			_button.onClick.RemoveAllListeners();
			_button.onClick.AddListener(() => OnSkillChoosen?.Invoke(skill));
			_skillNameLable.text = skill.SkillName;
			_skillLevelLable.text = skill.Level == 1 
					? _uiConfig.NewWeaponLevelText 
					: string.Format(_uiConfig.UpgradeWeaponLevelText, skill.Level);
			_skillDescription.text = skill.SkillDescription;
		}

	}

}