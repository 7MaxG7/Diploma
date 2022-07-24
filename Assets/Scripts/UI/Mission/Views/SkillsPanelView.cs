using UnityEngine;


namespace UI {

	internal class SkillsPanelView : MonoBehaviour {
		[SerializeField] private Transform _skillsUpgradeItemsContent;
		[SerializeField] private CanvasGroup _canvasGroup;

		public Transform SkillsUpgradeItemsContent => _skillsUpgradeItemsContent;
		public CanvasGroup CanvasGroup => _canvasGroup;
	}

}