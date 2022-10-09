using System;
using Infrastructure;
using UnityEngine;


namespace Weapons {

	[Serializable]
	internal sealed class UpgradeExpresstion {
		[SerializeField] private WeaponCharacteristicType _characteristicType;
		[SerializeField] private ArithmeticType _arithmetic;
		[SerializeField] private float _deltaValue;

		public WeaponCharacteristicType CharacteristicType => _characteristicType;
		public ArithmeticType Arithmetic => _arithmetic;
		public float DeltaValue => _deltaValue;
	}

}