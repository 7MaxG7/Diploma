using System.Collections.Generic;
using Units;
using UnityEngine;
using Zenject;


namespace Infrastructure {

	internal class MonstersMoveController : IMonstersMoveController {
		private Transform _target;
		private readonly List<IUnit> _enemies = new();


		[Inject]
		public MonstersMoveController(IControllersHolder controllersHolder) {
			controllersHolder.AddController(this);
		}
		
		public void OnUpdate(float deltaTime) {
			if (_target == null)
				return;
			
			_enemies.RemoveAll(enemy => enemy.IsDead);
			foreach (var enemy in _enemies) {
				enemy.CharacterController.Move((_target.position - enemy.CharacterController.transform.position).normalized * deltaTime * enemy.MoveSpeed);
			}
		}

		public void Init(Transform playerTransform) {
			_target = playerTransform;
		}

		public void RegisterMonster(IUnit enemy) {
			_enemies.Add(enemy);
		}

	}

}