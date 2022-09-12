using System;
using Infrastructure;
using Units;
using UnityEngine;


namespace Weapons {

	internal sealed class Weapon : IWeapon {
		public event Action<WeaponType> OnShooted;
		
		public WeaponType Type { get; }
		public float SqrRange { get; private set; }
		public int Level { get; private set; }
		public bool IsReady => _cooldownTimer <= 0;

		private readonly IUnit _owner;
		private IAmmosPool _ammosPool;
		private float _cooldown;
		private float _cooldownTimer;
		
		private readonly int[] _damage;
		private float _damageTickCooldown;
		private float _ammoSpeed;
		private bool _isPiercing;


		public Weapon(IUnit owner, WeaponBaseParam weaponBaseParam, IAmmosPool ammosPool) {
			_owner = owner;
			_ammosPool = ammosPool;
			Type = weaponBaseParam.WeaponType;
			SqrRange = weaponBaseParam.Range * weaponBaseParam.Range;
			_cooldown = weaponBaseParam.Cooldown;
			_damage = new int[weaponBaseParam.BaseTicksDamage.Length];
			Array.Copy(weaponBaseParam.BaseTicksDamage, _damage, weaponBaseParam.BaseTicksDamage.Length);
			_damageTickCooldown = weaponBaseParam.DamageTicksCooldown;
			_ammoSpeed = weaponBaseParam.AmmoSpeed;
			_isPiercing = weaponBaseParam.IsPiercing;
			Level = 1;
		}

		public void OnDispose() {
			_ammosPool = null;
		}

		public void ReduceCooldown(float deltaTime) {
			_cooldownTimer -= Math.Min(deltaTime, _cooldownTimer);
		}

		public void Shoot(Vector3 targetPosition) {
			_cooldownTimer += _cooldown;
			var ownerPosition = _owner.Transform.position;
			var ammo = _ammosPool.SpawnObject(ownerPosition, Type);
			ammo.Init(_owner, _damage, _damageTickCooldown, _isPiercing);
			ammo.Push((targetPosition - ownerPosition) * _ammoSpeed);
			OnShooted?.Invoke(Type);
		}

		public void Upgrade(WeaponLevelUpgradeParam upgradeParam) {
			if (upgradeParam == null)
				return;
			
			foreach (var upgrade in upgradeParam.Upgrades) {
				var values = GetValue(upgrade.CharacteristicType);
				if (values == null)
					return;
				
				for (var i = 0; i < values.Length; i++) {
					switch (upgrade.Arithmetic) {
						case ArithmeticType.Plus:
							values[i] += upgrade.DeltaValue;
							break;
						case ArithmeticType.Minus:
							values[i] -= upgrade.DeltaValue;
							break;
						case ArithmeticType.Multiply:
							values[i] *= upgrade.DeltaValue;
							break;
						case ArithmeticType.Divide:
							values[i] /= upgrade.DeltaValue;
							break;
						case ArithmeticType.Equal:
							values[i] = upgrade.DeltaValue;
							break;
						case ArithmeticType.None:
						default:
							return;
					}
				}
				SetValue(upgrade.CharacteristicType, values);

				Level = upgradeParam.Level;
			}
		}

		private float[] GetValue(WeaponCharacteristicType characteristicType) {
			switch (characteristicType) {
				case WeaponCharacteristicType.Damage:
					var values = new float[_damage.Length];
					for (var i = 0; i < _damage.Length; i++) {
						values[i] = _damage[i];
					}
					return values;
				case WeaponCharacteristicType.Range:
					return new[] { (float)Math.Sqrt(SqrRange) };
				case WeaponCharacteristicType.Cooldown:
					return new[] { _cooldown };
				case WeaponCharacteristicType.AmmoSpeed:
					return new[] { _ammoSpeed };
				case WeaponCharacteristicType.DamageTickCooldown:
					return new[] { _damageTickCooldown };
				case WeaponCharacteristicType.Pierciness:
					return new[] { _isPiercing ? 1f : 0f };
				case WeaponCharacteristicType.None:
				default:
					return null;
			}
		}

		private void SetValue(WeaponCharacteristicType characteristicType, float[] values) {
			switch (characteristicType) {
				case WeaponCharacteristicType.Damage:
					for (var i = 0; i < _damage.Length; i++) {
						_damage[i] = (int)values[i];
					}
					break;
				case WeaponCharacteristicType.Range:
					SqrRange = values[0] * values[0];
					break;
				case WeaponCharacteristicType.Cooldown:
					_cooldown = values[0];
					break;
				case WeaponCharacteristicType.AmmoSpeed:
					_ammoSpeed = values[0];
					break;
				case WeaponCharacteristicType.DamageTickCooldown:
					_damageTickCooldown = values[0];
					break;
				case WeaponCharacteristicType.Pierciness:
					_isPiercing = values[0] != 0;
					break;
				case WeaponCharacteristicType.None:
				default:
					break;
			}
		}
	}

}