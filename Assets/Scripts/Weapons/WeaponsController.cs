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
		private IUnit _player;
		private readonly List<IWeapon> _activeWeapons;
		private readonly List<IUnit> _monsters;
		private bool _isShooting;


		[Inject]
		public WeaponsController(IUnitsPool unitsPool, IAmmosPool ammosPool, WeaponsConfig weaponsConfig, IControllersHolder controllersHolder) {
			_ammosPool = ammosPool;
			_weaponsConfig = weaponsConfig;
			controllersHolder.AddController(this);
			_activeWeapons = new List<IWeapon>(weaponsConfig.WeaponsAmount);
			_monsters = unitsPool.ActiveMonsters;
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
		}

		public void StartShooting() {
			_isShooting = true;
		}

		public void AddWeapon(WeaponType type) {
			if (_activeWeapons.Any(weapon => weapon.Type == type))
				return;

			_activeWeapons.Add(new Weapon(_player, _weaponsConfig.GetWeaponBaseParam(type), _ammosPool));
		}

		public void StopShooting() {
			_isShooting = false;
		}
	}

}