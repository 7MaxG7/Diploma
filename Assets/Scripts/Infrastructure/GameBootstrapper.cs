using System;
using Controllers;
using UI;
using UnityEngine;
using Utils;


namespace Infrastructure {

	internal sealed class GameBootstrapper : MonoBehaviour, ICoroutineRunner {
		[SerializeField] private PermanentUiView _permanentUiView;
		
		private Game _game;


		private void Awake() {
			_game = new Game(this, _permanentUiView);
			var gameStateMachine = _game.GameStateMachine;
			gameStateMachine.GetState(typeof(LoadMissionState)).OnStateEntered += EnterRunMissionState;
			gameStateMachine.GetState(typeof(GameBootstrapState)).OnStateEntered += EnterLoadMissionState;
			gameStateMachine.Enter<GameBootstrapState>();
			
			DontDestroyOnLoad(this);


			void EnterLoadMissionState() {
				gameStateMachine.Enter<LoadMissionState, string>(TextConstants.MISSION_SCENE_NAME);
				gameStateMachine.GetState(typeof(GameBootstrapState)).OnStateEntered -= EnterLoadMissionState;
			}

			void EnterRunMissionState() {
				gameStateMachine.Enter<RunMissionState>();
				gameStateMachine.GetState(typeof(LoadMissionState)).OnStateEntered -= EnterRunMissionState;
			}
		}

		private void Update() {
			_game.Controllers.OnUpdate(Time.deltaTime);
		}

		private void LateUpdate() {
			_game.Controllers.OnLateUpdate(Time.deltaTime);
		}

		private void FixedUpdate() {
			_game.Controllers.OnFixedUpdate();
		}
	}

}