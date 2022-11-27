using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;


namespace Services
{
    internal sealed class PhotonManager : IPhotonManager
    {
        public string PlayerName
        {
            get => PhotonNetwork.LocalPlayer.NickName;
            set => PhotonNetwork.LocalPlayer.NickName = value;
        }

        public string RoomName => PhotonNetwork.CurrentRoom.Name;
        public bool IsMasterClient => PhotonNetwork.IsMasterClient;


        public void SubscribeCallbacks(object obj)
        {
            PhotonNetwork.AddCallbackTarget(obj);
        }

        public void UnsubscribeCallbacks(object obj)
        {
            PhotonNetwork.RemoveCallbackTarget(obj);
        }

#region Objects
        public void Destroy(PhotonView obj)
        {
            PhotonNetwork.Destroy(obj);
        }

        public bool AllocateViewID(PhotonView photonView)
        {
            return PhotonNetwork.AllocateViewID(photonView);
        }
#endregion

#region Lobby
        public void JoinCustonLobby()
        {
            PhotonNetwork.JoinLobby(new TypedLobby("customLobby", LobbyType.Default));
        }

        public void LeaveLobby()
        {
            PhotonNetwork.LeaveLobby();
        }

        public void Connect()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public void Disconnect()
        {
            PhotonNetwork.Disconnect();
        }
#endregion

#region Rooms
        public void SetRoomParameters(bool? isOpened = null, bool? isVisible = null)
        {
            if (isOpened.HasValue)
                PhotonNetwork.CurrentRoom.IsOpen = isOpened.Value;
            if (isVisible.HasValue)
                PhotonNetwork.CurrentRoom.IsVisible = isVisible.Value;
        }

        public int GetRoomPlayersAmount()
        {
            return PhotonNetwork.CurrentRoom.PlayerCount;
        }

        public int GetPlayerActorNumber()
        {
            return PhotonNetwork.LocalPlayer.ActorNumber;
        }

        public float GetLevelLoadingProgress()
        {
            return PhotonNetwork.LevelLoadingProgress;
        }

        public Dictionary<int, Player> GetRoomPlayers()
        {
            return PhotonNetwork.CurrentRoom.Players;
        }

        public void CreateRoom(string roomName, RoomOptions roomOptions)
        {
            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }

        public void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        public void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomOrCreateRoom();
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void LoadLevel(string sceneName)
        {
            PhotonNetwork.LoadLevel(sceneName);
        }
#endregion
    }
}