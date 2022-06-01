using System;
using Controllers;
using UnityEngine;
using Utils;
using Zenject;
using Object = UnityEngine.Object;


namespace Infrastructure {

	internal class LoadMissionState : ILoadMissionState {
		public event Action OnStateEntered;
		
		private readonly ISceneLoader _sceneLoader;
		private readonly IPermanentUiController _permanentUiController;
		private readonly IMoveController _moveController;
		private readonly ICameraController _cameraController;
		private UnitsFactory _unitsFactory;


		[Inject]
		public LoadMissionState(ISceneLoader sceneLoader, IPermanentUiController permanentUiController, IMoveController moveController, ICameraController cameraController) {
			_sceneLoader = sceneLoader;
			_permanentUiController = permanentUiController;
			_moveController = moveController;
			_cameraController = cameraController;
		}

		public void Enter(string sceneName) {
			_sceneLoader.LoadScene(sceneName, PrepareScene);

			
			void PrepareScene() {
				var player = Object.Instantiate(Resources.Load<CharacterController>(TextConstants.PLAYER_PREF_RESOURCES_PATH));
				// var player = _unitsFactory.CreatePlayer();

				_moveController.Init(player);
				_cameraController.Follow(player.transform, new Vector3(0, 0, -1));
				
				OnStateEntered?.Invoke();
			}
		}

		public void Exit() {
			_permanentUiController.HideLoadingCurtain();
		}
	}

}