using System;
using Units;
using UnityEngine;


namespace Utils {

	internal interface IUnitsFactory : IDisposable {
		IUnit CreatePlayer(Vector2 position);
		IUnit CreateMonster(int monsterParams, Vector2 spawnPosition);
	}

}