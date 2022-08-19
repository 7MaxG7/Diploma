using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using Units;
using UnityEngine;
using Utils;
using Zenject;


namespace Services {

	internal class UnitsPool : ObjectsPool<IUnit>, IUnitsPool {
		private readonly IUnitsFactory _unitsFactory;
		private readonly IHandleDamageController _handleDamageController;
		private readonly IMissionResultController _missionResultController;
		public List<IUnit> ActiveMonsters => _spawnedObjects;

		
		[Inject]
		public UnitsPool(IUnitsFactory unitsFactory, IHandleDamageController handleDamageController, IMissionResultController missionResultController) {
			_unitsFactory = unitsFactory;
			_handleDamageController = handleDamageController;
			_missionResultController = missionResultController;
		}

		public void Dispose() {
			foreach (var unit in _objects.Values.SelectMany(obj => obj)) {
				unit.OnDied -= ReturnUnit;
				unit.OnDied -= StopUnitPeriodicalDamage;
				unit.OnDied -= _missionResultController.CountKill;
				unit.Dispose();
			}
			foreach (var objList in _objects.Values) {
				objList.Clear();
			}
			_objects.Clear();
			foreach (var unit in ActiveMonsters) {
				unit.OnDied -= ReturnUnit;
				unit.OnDied -= StopUnitPeriodicalDamage;
				unit.OnDied -= _missionResultController.CountKill;
				unit.Dispose();
			}
			ActiveMonsters.Clear();
		}

		protected override int GetSpecifiedPoolIndex(object[] parameters) {
			return (int)parameters[0];
		}

		protected override IUnit SpawnSpecifiedObject(Vector2 spawnPosition, object[] parameters) {
			var currentMonsterLevel = (int)parameters[0];
			var unit = _unitsFactory.CreateMonster(currentMonsterLevel, spawnPosition);
			unit.OnDied += ReturnUnit;
			unit.OnDied += StopUnitPeriodicalDamage;
			unit.OnDied += _missionResultController.CountKill;
			return unit;
		}

		private void ReturnUnit(DamageInfo damageInfo) {
			ReturnObject(damageInfo.DamageTaker);
		}

		private void StopUnitPeriodicalDamage(DamageInfo damageInfo) {
			_handleDamageController.StopPeriodicalDamageForUnit(damageInfo.DamageTaker);
		}
	}

}