using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace UI {

	internal class SkillUiItemView : MonoBehaviour {
		[SerializeField] private GameObject _gameObject;
		[SerializeField] private Button _button;
		[SerializeField] private TMP_Text _skillNameLable;
		[SerializeField] private TMP_Text _skillLevelLable;
		[SerializeField] private TMP_Text _skillDescription;

		public GameObject GameObject => _gameObject;
		public Button Button => _button;
		public TMP_Text SkillNameLable => _skillNameLable;
		public TMP_Text SkillLevelLable => _skillLevelLable;
		public TMP_Text SkillDescription => _skillDescription;
	}

}