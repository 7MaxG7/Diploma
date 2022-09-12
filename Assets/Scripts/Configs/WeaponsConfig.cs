using System.Linq;
using UnityEngine;


namespace Weapons {

	[CreateAssetMenu(menuName = "Configs/" + nameof(WeaponsConfig), fileName = nameof(WeaponsConfig), order = 6)]
	internal class WeaponsConfig : ScriptableObject {
		
		[SerializeField] private WeaponBaseParam[] _weaponParams;
		[SerializeField] private WeaponUpgradeParam[] _weaponUpgradeParams;

		public int WeaponsAmount => _weaponParams.Length;


		public WeaponBaseParam GetWeaponBaseParam(WeaponType weaponType) {
			return _weaponParams.FirstOrDefault(param => param.WeaponType == weaponType);
		}

		public WeaponUpgradeParam GetWeaponUpgradeParam(WeaponType weaponType) {
			return _weaponUpgradeParams.FirstOrDefault(param => param.WeaponType == weaponType);
		}

		public WeaponType[] GetAllWeaponTypes() {
			return _weaponParams.Select(param => param.WeaponType).ToArray();
		}

		public int GetMaxLevelOfType(WeaponType weaponType) {
			var weaponUpgradeParams = _weaponUpgradeParams.FirstOrDefault(param => param.WeaponType == weaponType);
			return weaponUpgradeParams?.GetMaxLevel() ?? 1;
		}

	}

}