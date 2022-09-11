using Services;
using UnityEngine;


namespace Infrastructure {

	[CreateAssetMenu(menuName = "Configs/" + nameof(MissionConfig), fileName = nameof(MissionConfig), order = 2)]
	internal class MissionConfig : ScriptableObject {
		[SerializeField] private Vector3 _cameraOffset;
		[SerializeField] private int _baseChoosingSkillsAmount;
		[SerializeField] private GroundItemView _groundItemPref;
		[SerializeField] private string _photonDataSynchronizerPath;
		[Tooltip("Max distance for players fight logic")][SerializeField] private float _playersFightDistance;
		[Tooltip("Delay before final mission curtain in millisec")][SerializeField] private int _endMissionDelay;

		public int BaseChoosingSkillsAmount => _baseChoosingSkillsAmount;
		public GroundItemView GroundItemPref => _groundItemPref;
		public string PhotonDataSynchronizerPath => _photonDataSynchronizerPath;
		public Vector3 CameraOffset => _cameraOffset;
		public float PlayersFightDistance => _playersFightDistance;
		public int EndMissionDelay => _endMissionDelay;
	}

}