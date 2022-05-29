using System;
using Controllers;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;


namespace Infrastructure {

	internal class LoadMissionState : IParamedGameState<string> {
		public event Action OnStateEntered;
		
		private readonly SceneLoader _sceneLoader;
		private readonly ControllersBox _controllers;
		private readonly PermanentUiController _permanentUiController;
		private UnitsFactory _unitsFactory;


		public LoadMissionState(SceneLoader sceneLoader, ControllersBox controllers, PermanentUiController permanentUiController) {
			_sceneLoader = sceneLoader;
			_controllers = controllers;
			_permanentUiController = permanentUiController;
		}

		public void Enter(string sceneName) {
			_sceneLoader.LoadScene(sceneName, PrepareScene);

			
			void PrepareScene() {
				var player = _unitsFactory.CreatePlayer();

				var moveController = new MoveController(player, Game.InputService);
				moveController.Init();

				var cameraController = new CameraController();
				cameraController.Init();
				cameraController.Follow(player.transform, new Vector3(0, 0, -1));

				_controllers.AddController(moveController);
				_controllers.AddController(cameraController);
				
				OnStateEntered?.Invoke();
			}
		}

		public void Exit() {
			_permanentUiController.HideLoadingCurtain();
		}
	}

}