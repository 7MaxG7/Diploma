using System;
using Units;
using UnityEngine;


namespace Infrastructure {

	internal interface IMonstersMoveController : IFixedUpdater, IDisposable {
		void Init(Transform playerTransform);
		void RegisterMonster(IUnit enemy);
	}

}