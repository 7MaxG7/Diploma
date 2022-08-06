using System;
using System.Linq;
using UnityEngine;


namespace Infrastructure {

	[Serializable]
	internal class WeaponUpgradeParam : IWeaponDescription {
		[SerializeField] private WeaponType _weaponType;
		[SerializeField] private WeaponLevelUpgradeParam[] _weaponLevelUpgradeParams;

		public WeaponType WeaponType => _weaponType;
		
		
		public string GetNameForLevel(int level) {
			var upgradeParam = GetParamForLevel(level);
			return GetParamForLevel(level) == null ? string.Empty : upgradeParam.Name;
		}

		public WeaponLevelUpgradeParam GetUpgradeParamForLevel(int level) {
			return _weaponLevelUpgradeParams.FirstOrDefault(param => param.Level == level);
		}

		public string GetDescriptionForLevel(int level) {
			var upgradeParam = GetParamForLevel(level);
			return GetParamForLevel(level) == null ? string.Empty : upgradeParam.Description;
		}

		private WeaponLevelUpgradeParam GetParamForLevel(int level) {
			var upgradeParam = _weaponLevelUpgradeParams.FirstOrDefault(param => param.Level == level);
			return upgradeParam;
		}

		public int GetMaxLevel() {
			return _weaponLevelUpgradeParams.Select(param => param.Level).Max();
		}
	}

	[Serializable]
	internal class WeaponLevelUpgradeParam {
		[SerializeField] private int _weaponLevel;
		[SerializeField] private string _name;
		[SerializeField] private string _description;
		[SerializeField] private UpgradeExpresstion[] _upgrades;
		public int Level => _weaponLevel;
		public string Name => _name;
		public string Description => _description;
		public UpgradeExpresstion[] Upgrades => _upgrades;
	}

}