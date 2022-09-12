using System;
using UnityEngine;


namespace Weapons {

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