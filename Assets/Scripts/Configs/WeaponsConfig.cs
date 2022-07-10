using System;
using System.Linq;
using UnityEngine;


namespace Infrastructure {

	[CreateAssetMenu(menuName = "Configs/" + nameof(WeaponsConfig), fileName = nameof(WeaponsConfig), order = 6)]
	internal class WeaponsConfig : ScriptableObject {
		[Serializable]
		internal class WeaponParam {
			[SerializeField] private WeaponType weaponType;
			[SerializeField] private string _ammoPrefabPath;
			[SerializeField] private float _range;
			[SerializeField] private float _cooldown;
			[SerializeField] private float _ammoSpeed;
			[Tooltip("Damage of each tick in case of periodical damage")]
			[SerializeField] private int[] _baseDamage;
			[SerializeField] private float _damageTicksCooldown;
			[SerializeField] private bool _isPiercing;

			public WeaponType WeaponType => weaponType;
			public string AmmoPrefabPath => _ammoPrefabPath;
			public float Range => _range;
			public float Cooldown => _cooldown;
			public float AmmoSpeed => _ammoSpeed;
			public int[] BaseDamage => _baseDamage;
			public float DamageTicksCooldown => _damageTicksCooldown;
			public bool IsPiercing => _isPiercing;
		}

		public int WeaponsAmount => _ammoParams.Length;
		
		[SerializeField] private WeaponParam[] _ammoParams;


		public WeaponParam GetWeaponBaseParam(WeaponType weaponType) {
			return _ammoParams.FirstOrDefault(param => param.WeaponType == weaponType);
		}
	}

}