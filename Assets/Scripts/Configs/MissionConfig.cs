using UnityEngine;


namespace Infrastructure.Zenject {

	[CreateAssetMenu(menuName = "Configs/" + nameof(MissionConfig), fileName = nameof(MissionConfig), order = 2)]
	internal class MissionConfig : ScriptableObject {
		[SerializeField] private Transform _groundItemPref;
		
		public Transform GroundItemPref => _groundItemPref;
	}

}