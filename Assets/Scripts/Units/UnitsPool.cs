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
		public List<IUnit> ActiveMonsters => _spawnedObjects;

		
		[Inject]
		public UnitsPool(IUnitsFactory unitsFactory, IHandleDamageController handleDamageController) {
			_unitsFactory = unitsFactory;
			_handleDamageController = handleDamageController;
		}

		public void Dispose() {
			foreach (var unit in _objects.Values.SelectMany(obj => obj)) {
				unit.OnDied -= ReturnObject;
				unit.OnDied -= _handleDamageController.StopPeriodicalDamageForUnit;
				unit.Dispose();
			}
			foreach (var objList in _objects.Values) {
				objList.Clear();
			}
			_objects.Clear();
			foreach (var unit in ActiveMonsters) {
				unit.OnDied -= ReturnObject;
				unit.OnDied -= _handleDamageController.StopPeriodicalDamageForUnit;
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
			unit.OnDied += ReturnObject;
			unit.OnDied += _handleDamageController.StopPeriodicalDamageForUnit;
			return unit;
		}
	}

}