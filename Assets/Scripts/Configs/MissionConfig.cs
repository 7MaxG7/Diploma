using UnityEngine;


namespace Infrastructure {

	[CreateAssetMenu(menuName = "Configs/" + nameof(MissionConfig), fileName = nameof(MissionConfig), order = 2)]
	internal class MissionConfig : ScriptableObject {
		[SerializeField] private int _baseChoosingSkillsAmount;
		[SerializeField] private Transform _groundItemPref;
		[SerializeField] private string _photonDataSynchronizerPath;

		public int BaseChoosingSkillsAmount => _baseChoosingSkillsAmount;
		public Transform GroundItemPref => _groundItemPref;
		public string PhotonDataSynchronizerPath => _photonDataSynchronizerPath;
	}

}