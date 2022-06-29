using Units;
using UnityEngine;


namespace Infrastructure {

	internal interface IMonstersMoveController : IUpdater {
		void Init(Transform playerTransform);
		void RegisterMonster(IUnit enemy);
	}

}