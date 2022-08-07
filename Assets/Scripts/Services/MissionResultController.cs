using System;
using UI;
using Zenject;


namespace Infrastructure {

	internal class MissionResultController : IMissionResultController {
		public event Action OnGameLeft;

		private IPermanentUiController _permanentUiController;


		[Inject]
		public MissionResultController(IPermanentUiController permanentUiController) {
			_permanentUiController = permanentUiController;
		}

		public void Dispose() {
			_permanentUiController.OnLeaveGameClicked -= LeaveGame;
			_permanentUiController = null;
		}
		
		public void Init() {
			_permanentUiController.OnLeaveGameClicked += LeaveGame;
		}

		private void LeaveGame() {
			_permanentUiController.ShowMissionResult();
			OnGameLeft?.Invoke();
		}
	}

}
