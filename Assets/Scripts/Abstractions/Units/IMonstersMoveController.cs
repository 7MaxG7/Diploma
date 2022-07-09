using Units;
using UnityEngine;


namespace Infrastructure {

	internal interface IMonstersMoveController : IFixedUpdater {
		void Init(Transform playerTransform);
		void RegisterMonster(IUnit enemy);
	}

}