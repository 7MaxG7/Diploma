using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using Services;
using Units;
using UnityEngine;
using Utils;
using Zenject;
// ReSharper disable InconsistentNaming


namespace Infrastructure {

	internal sealed class MissionResultManager : IMissionResultManager, IInRoomCallbacks {
		public event Action OnGameLeft;
		public event Action<int> OnPlayerWithIdLeftRoomEvent;

		private readonly IPhotonManager _photonManager;
		private readonly IPlayfabManager _playfabManager;
		private readonly IPermanentUiController _permanentUiController;
		private readonly MissionConfig _missionConfig;
		private readonly Dictionary<IUnit, int> _unitsKills = new();
		private IUnit _player;
		private int _currentWinsAmount;
		private int _currentKillsAmount;


		[Inject]
		public MissionResultManager(IPhotonManager photonManager,IPlayfabManager playfabManager, IPermanentUiController permanentUiController, MissionConfig missionConfig) {
			_photonManager = photonManager;
			_playfabManager = playfabManager;
			_permanentUiController = permanentUiController;
			_missionConfig = missionConfig;
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
			OnPlayerWithIdLeftRoomEvent?.Invoke(otherPlayer.ActorNumber);
			if (_photonManager.GetRoomPlayersAmount() == 1) {
				WinGame();
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
				_playfabManager.SetData(
						new Dictionary<string, string> { { Constants.KILLS_AMOUNT_PLAYFAB_KEY, (++_currentKillsAmount).ToString() } }
						, null
						, OnErrorResult
				);
			}

			void OnErrorResult(PlayFabError error) {
				Debug.LogWarning(error.GenerateErrorReport());
			}
		}

		public void SetWinsAmount(int winsAmount) {
			_currentWinsAmount = winsAmount;
		}

		public void SetKillsAmount(int killsAmount) {
			_currentKillsAmount = killsAmount;
		}

		private void WinGame() {
			_playfabManager.SetData(
					new Dictionary<string, string> { { Constants.WINS_AMOUNT_PLAYFAB_KEY, (++_currentWinsAmount).ToString() } }
					, null
					, OnErrorResult
			);
			ShowVictoryResultAsync();

			void OnErrorResult(PlayFabError error) {
				Debug.LogWarning(error.GenerateErrorReport());
			}
		}

		private async void LooseGame(DamageInfo _) {
			await Task.Delay(_missionConfig.EndMissionDelay);
			LeaveGame();
		}

		private void LeaveGame() {
			var unitKills = _unitsKills.ContainsKey(_player) ? _unitsKills[_player] : 0;
			var missionEndInfo = new MissionEndInfo(false, unitKills);
			EndGame(missionEndInfo);
		}

		private async Task ShowVictoryResultAsync() {
			await Task.Delay(500);
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
