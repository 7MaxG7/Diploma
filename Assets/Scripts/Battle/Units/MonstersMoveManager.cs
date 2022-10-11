using System.Collections.Generic;
using Units;
using UnityEngine;
using Zenject;


namespace Infrastructure {

	internal sealed class MonstersMoveManager : IMonstersMoveManager {
		private Transform _target;
		private readonly List<IUnit> _enemies = new();


		[Inject]
		public MonstersMoveManager(IControllersHolder controllersHolder) {
			controllersHolder.AddController(this);
		}

		public void Dispose() {
			_enemies.Clear();
			_target = null;
		}

		public void OnFixedUpdate(float deltaTime) {
			if (_target == null)
				return;
			
			_enemies.RemoveAll(enemy => enemy.IsDead);
			foreach (var enemy in _enemies) {
				enemy.Move((_target.position - enemy.Transform.position).normalized * (deltaTime * enemy.MoveSpeed));
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