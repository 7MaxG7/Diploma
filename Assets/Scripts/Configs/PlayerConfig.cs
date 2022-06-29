using UnityEngine;


namespace Infrastructure {

	[CreateAssetMenu(menuName = "Configs/" + nameof(PlayerConfig), fileName = nameof(PlayerConfig), order = 4)]
	internal class PlayerConfig : ScriptableObject {
		[SerializeField] private float _baseMoveSpeed;
		[SerializeField] private int _baseHp;

		public float BaseMoveSpeed => _baseMoveSpeed;
		public int BaseHp => _baseHp;
	}

}