using Abstractions;
using Abstractions.Services;
using Infrastructure;
using UI;
using Units;
using UnityEngine;
using Zenject;


namespace Services {

	internal class CompassController : ICompassController {
		private readonly IInputService _inputService;
		private readonly IPlayersInteractionController _playersInteractionController;
		private readonly IMissionUiController _missionUiController;
		private readonly IControllersHolder _controllersHolder;

		private IUnit _player;

		
		[Inject]
		public CompassController(IInputService inputService, IPlayersInteractionController playersInteractionController
				, IMissionUiController missionUiController, IControllersHolder controllersHolder) {
			_inputService = inputService;
			_playersInteractionController = playersInteractionController;
			_missionUiController = missionUiController;
			_controllersHolder = controllersHolder;
		}
		
		public void Dispose() {
			_player = null;
			_controllersHolder.RemoveController(this);
		}

		public void OnUpdate(float deltaTime) {
			if (_player == null || _playersInteractionController.ClosestFightingEnemyPlayer == null || _missionUiController == null)
				return;
			
			if (_inputService.CompassButtonIsPressed)
				ShowCompass();
			else {
				HideCompass();
			}
		}

		public void Init(IUnit player) {
			if (!_playersInteractionController.IsMultiplayerGame.HasValue) {
				Debug.LogWarning("_playersInteractionController.IsMultiplayerGame is not inited");
				return;
			}
			
			if (!_playersInteractionController.IsMultiplayerGame.Value)
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
			return (_playersInteractionController.ClosestFightingEnemyPlayer.Transform.position - _player.Transform.position).normalized;
		}

	}

}