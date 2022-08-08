using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Units;
using Zenject;


namespace Infrastructure {

	internal class MissionResultController : IMissionResultController, IInRoomCallbacks {
		public event Action OnGameLeft;

		private readonly IPermanentUiController _permanentUiController;
		private readonly Dictionary<IUnit, int> _unitsKills = new();
		private IUnit _player;


		[Inject]
		public MissionResultController(IPermanentUiController permanentUiController) {
			_permanentUiController = permanentUiController;
		}

		public void Dispose() {
			_permanentUiController.OnLeaveGameClicked -= LeaveGame;
			_unitsKills.Clear();
			_player.OnDied -= LooseGame;
			_player = null;
		}

#region IInRoomCallbacksMethods
		public void OnPlayerLeftRoom(Player otherPlayer) {
			if (PhotonNetwork.CurrentRoom.PlayerCount == 1) {
				ShowVictoryResult();
			}
		}

		public void OnPlayerEnteredRoom(Player newPlayer) { }

		public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) { }

		public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) { }

		public void OnMasterClientSwitched(Player newMasterClient) { }
#endregion
		
		public void Init(IUnit player) {
			_player = player;
			_player.OnDied += LooseGame;
			_permanentUiController.OnLeaveGameClicked += LeaveGame;
		}

		public void CountDead(DamageInfo damageInfo) {
			// Damager info can be null id killed by level up
			if (damageInfo.Damager == null)
				return;
			
			if (!_unitsKills.ContainsKey(damageInfo.Damager))
				_unitsKills.Add(damageInfo.Damager, 0);
			_unitsKills[damageInfo.Damager]++;
		}

		private void LooseGame(DamageInfo info) {
			LeaveGame();
		}

		private void LeaveGame() {
			var unitKills = _unitsKills.ContainsKey(_player) ? _unitsKills[_player] : 0;
			var missionEndInfo = new MissionEndInfo(false, unitKills);
			EndGame(missionEndInfo);
		}

		private void ShowVictoryResult() {
			var unitKills = _unitsKills.ContainsKey(_player) ? _unitsKills[_player] : 0;
			var missionEndInfo = new MissionEndInfo(true, unitKills);
			EndGame(missionEndInfo);
		}

		private void EndGame(MissionEndInfo missionEndInfo) {
			_permanentUiController.ShowMissionResult(missionEndInfo);
			OnGameLeft?.Invoke();
		}

	}

}
