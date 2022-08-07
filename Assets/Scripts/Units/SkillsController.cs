using System;
using System.Collections.Generic;
using System.Linq;
using Services;
using UI;
using Units;
using Zenject;


namespace Infrastructure {

	internal class SkillsController : ISkillsController {
		private int ChoosingSkillsAmount { get; }
		private IWeaponsController _weaponsController;
		private IRandomController _randomController;
		private IMissionUiController _missionUiController;
		private IMonstersSpawner _monstersSpawner;
		private IUnitsPool _unitsPool;
		private readonly WeaponsConfig _weaponsConfig;
		private IUnit _player;

		[Inject]
		public SkillsController(IWeaponsController weaponsController, IRandomController randomController, IMissionUiController missionUiController
				, IMonstersSpawner monstersSpawner, IUnitsPool unitsPool, WeaponsConfig weaponsConfig, MissionConfig missionConfig) {
			_weaponsController = weaponsController;
			_randomController = randomController;
			_missionUiController = missionUiController;
			_monstersSpawner = monstersSpawner;
			_unitsPool = unitsPool;
			_weaponsConfig = weaponsConfig;
			ChoosingSkillsAmount = missionConfig.BaseChoosingSkillsAmount;
		}
		
		public void Dispose() {
			_player.Experience.OnLevelUp -= PrepareSkillsForUpgrade;
			_missionUiController.OnSkillChoose -= AddOrUpgradeSkill;
			_player = null;
			_weaponsController = null;
			_randomController = null;
			_missionUiController = null;
			_monstersSpawner = null;
			_unitsPool = null;
		}

		public void Init(IUnit player) {
			_player = player;
			_player.Experience.OnLevelUp += PrepareSkillsForUpgrade;
			_missionUiController.OnSkillChoose += AddOrUpgradeSkill;
		}

		private void PrepareSkillsForUpgrade(int playerLevel) {
			var randomSkillIndexes = new List<int>(ChoosingSkillsAmount);
			int randomSkillIndex;
			for (var i = 0; i < Math.Min(ChoosingSkillsAmount, _weaponsController.UpgradableWeaponTypes.Count); i++) {
				do {
					randomSkillIndex = _randomController.GetRandomExcludingMax(_weaponsController.UpgradableWeaponTypes.Count);
				} while (randomSkillIndexes.Contains(randomSkillIndex));
				randomSkillIndexes.Add(randomSkillIndex);
			}
			var randomSkillTypes = randomSkillIndexes.Select(skillIndex => _weaponsController.UpgradableWeaponTypes[skillIndex]).ToArray();
			var choosingSkills = new ActualSkillInfo[randomSkillTypes.Length];
			for (var i = 0; i < randomSkillTypes.Length; i++) {
				choosingSkills[i] = new ActualSkillInfo(randomSkillTypes[i], _weaponsController.GetCurrentLevelOfWeapon(randomSkillTypes[i]) + 1);
				SetupSkillInfo(choosingSkills[i]);
			}

			for (var i = _unitsPool.ActiveMonsters.Count - 1; i >= 0; i--) {
				_unitsPool.ActiveMonsters[i].KillUnit();
			}
			if (choosingSkills.Length > 0) {
				_missionUiController.ShowSkillsChoose(choosingSkills);
				_monstersSpawner.StopSpawn();
			}
		}

		private void SetupSkillInfo(ActualSkillInfo skillInfo) {
			IWeaponDescription skillParams;
			if (skillInfo.Level == 1)
				skillParams = _weaponsConfig.GetWeaponBaseParam(skillInfo.WeaponType);
			else {
				skillParams = _weaponsConfig.GetWeaponUpgradeParam(skillInfo.WeaponType);
			}
			skillInfo.Setup(skillParams);
		}

		private void AddOrUpgradeSkill(WeaponType weaponType) {
			_weaponsController.AddOrUpgradeWeapon(weaponType);
			
			_monstersSpawner.StartSpawn();
		}
	}

}