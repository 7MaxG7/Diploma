using System;
using System.Collections.Generic;
using System.Linq;
using Services;
using UI;
using Units;
using Zenject;


namespace Infrastructure {

	internal class SkillsController : ISkillsController {
		private readonly IWeaponsController _weaponsController;
		private readonly IRandomController _randomController;
		private readonly IMissionUiController _missionUiController;
		private readonly IMonstersSpawner _monstersSpawner;
		private readonly WeaponsConfig _weaponsConfig;
		private int ChoosingSkillsAmount { get; }
		private IUnit _player;
		private bool _spawnerIsTurnedOffHere;

		[Inject]
		public SkillsController(IWeaponsController weaponsController, IRandomController randomController, IMissionUiController missionUiController
				, IMonstersSpawner monstersSpawner, WeaponsConfig weaponsConfig, MissionConfig missionConfig) {
			_weaponsController = weaponsController;
			_randomController = randomController;
			_missionUiController = missionUiController;
			_monstersSpawner = monstersSpawner;
			_weaponsConfig = weaponsConfig;
			ChoosingSkillsAmount = missionConfig.BaseChoosingSkillsAmount;
		}
		
		public void Dispose() {
			_player.Experience.OnLevelUp -= PrepareSkillsForUpgrade;
			_missionUiController.OnSkillChoose -= AddOrUpgradeSkill;
			_player = null;
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

			if (choosingSkills.Length > 0) {
				_missionUiController.ShowSkillsChoose(choosingSkills);
				if (_monstersSpawner.SpawnIsOn) {
					_monstersSpawner.KillMonstersAndStopSpawn();
					_spawnerIsTurnedOffHere = true;
				} 
				_monstersSpawner.KillMonstersAndStopSpawn();
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
			if (_spawnerIsTurnedOffHere) {
				_monstersSpawner.StartSpawn();
				_spawnerIsTurnedOffHere = false;
			}
		}
	}

}