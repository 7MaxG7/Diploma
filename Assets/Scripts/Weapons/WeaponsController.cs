using System.Collections.Generic;
using System.Linq;
using Services;
using Units;
using UnityEngine;
using Zenject;


namespace Infrastructure {

	internal class WeaponsController : IWeaponsController {
		private readonly IAmmosPool _ammosPool;
		private readonly WeaponsConfig _weaponsConfig;
		private readonly List<IWeapon> _activeWeapons;
		private readonly List<IUnit> _monsters;
		private readonly ISoundController _soundController;
		public List<WeaponType> UpgradableWeaponTypes { get; private set; }
		private IUnit _player;
		private bool _isShooting;


		[Inject]
		public WeaponsController(IUnitsPool unitsPool, IAmmosPool ammosPool, ISoundController soundController, WeaponsConfig weaponsConfig
				, IControllersHolder controllersHolder) {
			_ammosPool = ammosPool;
			_soundController = soundController;
			_weaponsConfig = weaponsConfig;
			_activeWeapons = new List<IWeapon>(weaponsConfig.WeaponsAmount);
			_monsters = unitsPool.ActiveMonsters;
			controllersHolder.AddController(this);
		}

		public void Dispose() {
			StopShooting();
			foreach (var weapon in _activeWeapons) {
				weapon.OnShooted -= _soundController.PlayWeaponShootSound;
			}
			_activeWeapons.Clear();
			_player = null;
		}

		public void OnUpdate(float deltaTime) {
			if (!_isShooting)
				return;
			
			var readyWeapons = new List<IWeapon>(_activeWeapons.Count);
			foreach (var weapon in _activeWeapons) {
				weapon.ReduceCooldown(deltaTime);
				if (weapon.IsReady)
					readyWeapons.Add(weapon);
			}
			if (readyWeapons.Count == 0)
				return;

			IUnit target = null;
			var minSqrMagnitude = float.MaxValue;
			foreach (var monster in _monsters.Where(unit => !unit.IsDead)) {
				var currentSqrMagnitude = Vector3.SqrMagnitude(_player.Transform.position - monster.Transform.position);
				if (currentSqrMagnitude < minSqrMagnitude) {
					target = monster;
					minSqrMagnitude = currentSqrMagnitude;
				}
			}
			if (target == null)
				return;
			
			readyWeapons = readyWeapons.Where(item => item.SqrRange >= minSqrMagnitude).ToList();
			foreach (var weapon in readyWeapons) {
				weapon.Shoot(target);
			}
		}

		public void Init(IUnit player) {
			_player = player;
			UpgradableWeaponTypes = _weaponsConfig.GetAllWeaponTypes().ToList();
		}

		public void StartShooting() {
			_isShooting = true;
		}

		public void StopShooting() {
			_isShooting = false;
		}

		public void AddOrUpgradeWeapon(WeaponType weaponType) {
			var desiredWeapon = _activeWeapons.FirstOrDefault(weapon => weapon.Type == weaponType);
			if (desiredWeapon == null) {
				AddWeapon(weaponType);
			} else {
				UpgradeActiveWeapon(desiredWeapon);
			}
			

			void UpgradeActiveWeapon(IWeapon upgradingWeapon) {
				var upgradeParam = _weaponsConfig.GetWeaponUpgradeParam(upgradingWeapon.Type);
				upgradingWeapon.Upgrade(upgradeParam?.GetUpgradeParamForLevel(upgradingWeapon.Level + 1));
				
				UpdateUpgradableWeaponsList(upgradingWeapon);
			}
		}

		public void AddWeapon(WeaponType type) {
			if (_activeWeapons.Any(weapon => weapon.Type == type))
				return;

			var newWeapon = new Weapon(_player, _weaponsConfig.GetWeaponBaseParam(type), _ammosPool);
			newWeapon.OnShooted += _soundController.PlayWeaponShootSound;
			_activeWeapons.Add(newWeapon);
			UpdateUpgradableWeaponsList(newWeapon);
		}

		public int GetCurrentLevelOfWeapon(WeaponType weaponType) {
			var requiredWeapon = _activeWeapons.FirstOrDefault(weapon => weapon.Type == weaponType);
			if (requiredWeapon == null)
				return 0;

			return requiredWeapon.Level;
		}

		private void UpdateUpgradableWeaponsList(IWeapon newWeapon) {
			if (_weaponsConfig.GetMaxLevelOfType(newWeapon.Type) <= newWeapon.Level)
				UpgradableWeaponTypes.Remove(newWeapon.Type);
		}
	}

}