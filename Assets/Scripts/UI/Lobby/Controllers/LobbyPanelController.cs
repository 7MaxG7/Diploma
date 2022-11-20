using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using Services;
using UI;


namespace Infrastructure
{
    internal sealed class LobbyPanelController : IDisposable
    {
        private readonly IPhotonManager _photonManager;
        private readonly LobbyPanelView _lobbyPanelView;
        private readonly MainMenuConfig _mainMenuConfig;
        private readonly ILobbyStatusDisplayer _lobbyStatusDisplayer;
        private readonly IViewsFactory _viewsFactory;
        private readonly List<RoomInfo> _roomsList = new();
        private bool _uiIsBlocked;
        private IRoomEventsCallbacks _lobbyScreenController;
        private bool _isInited;


        public LobbyPanelController(IPhotonManager photonManager, MainMenuConfig mainMenuConfig,
            LobbyPanelView lobbyPanelView, ILobbyStatusDisplayer lobbyStatusDisplayer, IViewsFactory viewsFactory)
        {
            _photonManager = photonManager;
            _lobbyPanelView = lobbyPanelView;
            _mainMenuConfig = mainMenuConfig;
            _lobbyStatusDisplayer = lobbyStatusDisplayer;
            _viewsFactory = viewsFactory;
        }

        public void Dispose()
        {
            OnDispose();
        }

        public void OnDispose()
        {
            if (!_isInited)
                return;

            _isInited = false;
            DOTween.KillAll();
            _lobbyPanelView.OnDispose();
            _lobbyPanelView.OnCreatePrivateRoomClick -= CreatePrivateRoom;
            _lobbyPanelView.OnJoinPrivateRoomClick -= JoinRoom;
            _lobbyPanelView.OnCreateNewRoomClick -= CreateRoom;
            _lobbyPanelView.OnJoinRandomRoomClick -= JoinOrCreateRandomRoom;
            _lobbyPanelView.OnClosePanelClick -= Disconnect;
            _lobbyPanelView.OnJoinRoomClick -= JoinRoom;
            _lobbyScreenController.OnLobbyJoin -= _lobbyPanelView.UnblockUi;
            _lobbyScreenController.OnRoomCreationFail -= _lobbyPanelView.UnblockUi;
            _lobbyScreenController.OnRoomJoinFail -= _lobbyPanelView.UnblockUi;
            _lobbyScreenController.OnRandomRoomJoinFail -= _lobbyPanelView.UnblockUi;
        }

        public void Init(IRoomEventsCallbacks lobbyScreenController)
        {
            _lobbyScreenController = lobbyScreenController;
            _lobbyPanelView.Init(_viewsFactory, _mainMenuConfig);
            _lobbyPanelView.OnCreatePrivateRoomClick += CreatePrivateRoom;
            _lobbyPanelView.OnJoinPrivateRoomClick += JoinRoom;
            _lobbyPanelView.OnCreateNewRoomClick += CreateRoom;
            _lobbyPanelView.OnJoinRandomRoomClick += JoinOrCreateRandomRoom;
            _lobbyPanelView.OnClosePanelClick += Disconnect;
            _lobbyPanelView.OnJoinRoomClick += JoinRoom;
            _lobbyScreenController.OnLobbyJoin += _lobbyPanelView.UnblockUi;
            _lobbyScreenController.OnRoomCreationFail += _lobbyPanelView.UnblockUi;
            _lobbyScreenController.OnRoomJoinFail += _lobbyPanelView.UnblockUi;
            _lobbyScreenController.OnRandomRoomJoinFail += _lobbyPanelView.UnblockUi;
            _isInited = true;
        }

        public void ShowPanel(Action onPanelShownCallback = null)
        {
            _lobbyPanelView.Show(onPanelShownCallback);
        }

        public void HidePanel(Action onPanelHiddenCallback = null)
        {
            _lobbyPanelView.Hide(onPanelHiddenCallback: onPanelHiddenCallback);
        }

        public void DeactivatePanel()
        {
            _lobbyPanelView.Hide(false);
        }

        public void UpdateRoomsList(List<RoomInfo> roomList)
        {
            foreach (var roomInfo in roomList)
            {
                if (roomInfo.RemovedFromList)
                {
                    if (_roomsList.Remove(roomInfo))
                    {
                        _lobbyPanelView.RemoveRoom(roomInfo);
                    }
                }
                else
                {
                    if (!_roomsList.Contains(roomInfo))
                    {
                        _lobbyPanelView.AddRoom(roomInfo);
                        _roomsList.Add(roomInfo);
                    }
                }
            }
        }

        private void CreatePrivateRoom(string roomName)
        {
            _lobbyStatusDisplayer.ShowLoadingStatusAsync();
            var roomOptions = new RoomOptions { IsVisible = false };
            _photonManager.CreateRoom(roomName, roomOptions);
        }

        private void CreateRoom()
        {
            var roomName = string.Format(_mainMenuConfig.RoomNameTemplate, _photonManager.PlayerName,
                (int)PhotonNetwork.Time);
            var currentRoomsNames = _roomsList.Select(roomInfo => roomInfo.Name).ToArray();
            if (currentRoomsNames.Contains(roomName))
            {
                var additionalIndex = 0;
                while (currentRoomsNames.Contains($"{roomName} {additionalIndex}"))
                {
                    ++additionalIndex;
                }

                roomName += $" {additionalIndex}";
            }

            _lobbyStatusDisplayer.ShowLoadingStatusAsync();
            _photonManager.CreateRoom(roomName, new RoomOptions());
        }

        private void JoinRoom(string roomName)
        {
            _lobbyStatusDisplayer.ShowLoadingStatusAsync();
            _photonManager.JoinRoom(roomName);
        }

        private void JoinOrCreateRandomRoom()
        {
            _lobbyStatusDisplayer.ShowLoadingStatusAsync();
            _photonManager.JoinRandomRoom();
        }

        private void Disconnect()
        {
            _lobbyStatusDisplayer.ShowLoadingStatusAsync();
            _photonManager.LeaveLobby();
        }
    }
}