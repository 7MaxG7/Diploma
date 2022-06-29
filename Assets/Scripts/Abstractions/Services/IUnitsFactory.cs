﻿using Infrastructure;
using Units;
using UnityEngine;


namespace Utils {

	internal interface IUnitsFactory {
		IUnit CreatePlayer(Vector2 position);
		IUnit CreateMonster(int monsterParams, Vector2 spawnPosition);
	}

}