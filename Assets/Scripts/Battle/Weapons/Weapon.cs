using System;
using Abstractions.Utils;
using Units;
using UnityEngine;


namespace Weapons {

	internal sealed class Weapon : IWeapon {
		public event Action<WeaponType> OnShooted;
		
		public WeaponType Type { get; }
		public float SqrRange => _weaponStats.SqrRange;
		public int Level { get; private set; }
		public bool IsReady => _cooldownTimer <= 0;

		private IWeaponStats _weaponStats;
		private readonly IUnit _owner;
		private IAmmosPool _ammosPool;
		private readonly IUpgradedWeaponsFactory _upgradedWeaponsFactory;
		private float _cooldownTimer;


		public Weapon(IUnit owner, WeaponBaseParam weaponBaseParam, IAmmosPool ammosPool, IUpgradedWeaponsFactory upgradedWeaponsFactory)
		{
			_owner = owner;
			_ammosPool = ammosPool;
			_upgradedWeaponsFactory = upgradedWeaponsFactory;
			Type = weaponBaseParam.WeaponType;
			_weaponStats = new BaseWeaponStats(weaponBaseParam);
			Level = 1;
		}

		public void OnDispose() {
			_ammosPool = null;
		}

		public void ReduceCooldown(float deltaTime) {
			_cooldownTimer -= Math.Min(deltaTime, _cooldownTimer);
		}

		public void Shoot(Vector3 targetPosition) {
			_cooldownTimer += _weaponStats.Cooldown;
			var ownerPosition = _owner.Transform.position;
			var ammo = _ammosPool.SpawnObject(ownerPosition, Type);
			ammo.Init(_owner, _weaponStats.Damage, _weaponStats.DamageTicksCooldown, _weaponStats.IsPiercing);
			ammo.Push((targetPosition - ownerPosition) * _weaponStats.AmmoSpeed);
			OnShooted?.Invoke(Type);
		}

		public void Upgrade(WeaponLevelUpgradeParam upgradeParam) {
			if (upgradeParam == null)
				return;
			
			_weaponStats = _upgradedWeaponsFactory.CreatedUpgradeWeaponStats(_weaponStats, upgradeParam);
			Level = upgradeParam.Level;
		}
	}
}