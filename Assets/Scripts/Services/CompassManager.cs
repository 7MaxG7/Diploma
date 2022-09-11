using Abstractions;
using Infrastructure;
using UI;
using Units;
using UnityEngine;
using Zenject;


namespace Services {

	internal sealed class CompassManager : ICompassManager {
		private readonly IInputService _inputService;
		private readonly IPlayersInteractionManager _playersInteractionManager;
		private readonly IMissionUiController _missionUiController;
		private readonly IControllersHolder _controllersHolder;

		private IUnit _player;

		
		[Inject]
		public CompassManager(IInputService inputService, IPlayersInteractionManager playersInteractionManager
				, IMissionUiController missionUiController, IControllersHolder controllersHolder) {
			_inputService = inputService;
			_playersInteractionManager = playersInteractionManager;
			_missionUiController = missionUiController;
			_controllersHolder = controllersHolder;
		}
		
		public void Dispose() {
			_player = null;
			_controllersHolder.RemoveController(this);
		}

		public void OnUpdate(float deltaTime) {
			if (_player == null || _playersInteractionManager.ClosestFightingEnemyPlayer == null || _missionUiController == null)
				return;
			
			if (_inputService.CompassButtonIsPressed)
				ShowCompass();
			else {
				HideCompass();
			}
		}

		public void Init(IUnit player) {
			if (!_playersInteractionManager.IsMultiplayerGame.HasValue) {
				Debug.LogWarning("_playersInteractionController.IsMultiplayerGame is not inited");
				return;
			}
			
			if (!_playersInteractionManager.IsMultiplayerGame.Value)
				return;
			
			_player = player;
			_controllersHolder.AddController(this);
		}

		private void ShowCompass() {
			_missionUiController.ShowCompass(GetClosestEnemyPlayerDirection());
		}

		private void HideCompass() {
			_missionUiController.HideCompass(GetClosestEnemyPlayerDirection());
		}

		private Vector3 GetClosestEnemyPlayerDirection() {
			return (_playersInteractionManager.ClosestFightingEnemyPlayer.Transform.position - _player.Transform.position).normalized;
		}

	}

}