using System;
using Units;
using UnityEngine;


namespace Infrastructure {

	internal class Weapon : IWeapon {
		public WeaponType Type { get; }
		public float SqrRange { get; }
		public bool IsReady => _cooldownTimer <= 0;
		
		private float _cooldown;
		private float _ammoSpeed;
		private float _cooldownTimer;
		private readonly IUnit _owner;
		private readonly IAmmosPool _ammosPool;


		public Weapon(IUnit owner, WeaponsConfig.WeaponParam weaponBaseParam, IAmmosPool ammosPool) {
			_owner = owner;
			_ammosPool = ammosPool;
			Type = weaponBaseParam.WeaponType;
			SqrRange = weaponBaseParam.Range * weaponBaseParam.Range;
			_cooldown = weaponBaseParam.Cooldown;
			_ammoSpeed = weaponBaseParam.AmmoSpeed;
		}


		public void ReduceCooldown(float deltaTime) {
			_cooldownTimer -= Math.Min(deltaTime, _cooldownTimer);
		}

		public void Shoot(IUnit target) {
			_cooldownTimer += _cooldown;
			var ownerPosition = _owner.Transform.position;
			var ammo = _ammosPool.SpawnObject(ownerPosition, Type);
			ammo.Init(_owner);
			var destination = target.Transform.position - ownerPosition;
			ammo.RigidBody.AddForce(destination * _ammoSpeed, ForceMode2D.Impulse);
		}
	}

}