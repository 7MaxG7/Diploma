using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using Units;
using UnityEngine;
using Utils;
using Zenject;


namespace Infrastructure {

	internal class MissionResultController : IMissionResultController, IInRoomCallbacks {
		public event Action OnGameLeft;
		public event Action OnPlayerLeftRoomEvent;

		private readonly IPermanentUiController _permanentUiController;
		private readonly Dictionary<IUnit, int> _unitsKills = new();
		private IUnit _player;
		private int _currentWinsAmount;
		private int _currentKillsAmount;


		[Inject]
		public MissionResultController(IPermanentUiController permanentUiController) {
			_permanentUiController = permanentUiController;
		}

		public void Dispose() {
			_permanentUiController.OnLeaveGameClicked -= LeaveGame;
			_unitsKills.Clear();
			_player.OnDied -= LooseGame;
			_player = null;
			PhotonNetwork.RemoveCallbackTarget(this);
		}

#region IInRoomCallbacksMethods
		public void OnPlayerLeftRoom(Player otherPlayer) {
			OnPlayerLeftRoomEvent?.Invoke();
			if (PhotonNetwork.CurrentRoom.PlayerCount == 1) {
				PlayFabClientAPI.UpdateUserData(
						new UpdateUserDataRequest { 
								Data = new Dictionary<string, string> { { TextConstants.WINS_AMOUNT_PLAYFAB_KEY, (++_currentWinsAmount).ToString() } }
						}, null
						, errorCallback => Debug.LogWarning(errorCallback.GenerateErrorReport())
				);
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
			PhotonNetwork.AddCallbackTarget(this);
		}

		public void CountKill(DamageInfo damageInfo) {
			// Damager info can be null if killed with level up
			if (damageInfo.Damager == null)
				return;
			
			if (!_unitsKills.ContainsKey(damageInfo.Damager))
				_unitsKills.Add(damageInfo.Damager, 0);
			_unitsKills[damageInfo.Damager]++;
			if (damageInfo.Damager == _player) {
				PlayFabClientAPI.UpdateUserData(
						new UpdateUserDataRequest { 
								Data = new Dictionary<string, string> { { TextConstants.KILLS_AMOUNT_PLAYFAB_KEY, (++_currentKillsAmount).ToString() } }
						}, null
						, errorCallback => Debug.LogWarning(errorCallback.GenerateErrorReport())
				);			
			}
		}

		public void SetWinsAmount(int winsAmount) {
			_currentWinsAmount = winsAmount;
		}

		public void SetKillsAmount(int killsAmount) {
			_currentKillsAmount = killsAmount;
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
