using System.Collections.Generic;
using Units;
using UnityEngine;
using Utils;
using Zenject;


namespace Services {

	internal class UnitsPool : ObjectsPool<IUnit>, IUnitsPool {
		private readonly IUnitsFactory _unitsFactory;
		public List<IUnit> ActiveMonsters => _spawnedObjects;

		
		[Inject]
		public UnitsPool(IUnitsFactory unitsFactory) {
			_unitsFactory = unitsFactory;
			_unitsFactory.SetUnitsPool(this);
		}
		
		protected override IUnit SpawnSpecifiedObject(Vector2 spawnPosition, object[] parameters) {
			var currentMonsterLevel = (int)parameters[0];
			return _unitsFactory.CreateMonster(currentMonsterLevel, spawnPosition);
		}

	}

}