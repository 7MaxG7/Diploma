using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


namespace Services
{
    internal interface IPhotonManager
    {
        string PlayerName { get; set; }
        string RoomName { get; }
        bool IsMasterClient { get; }

        GameObject Create(string prefabPath, Vector2 position, Quaternion rotation);
        void Destroy(PhotonView obj);
        void CreateRoom(string roomName, RoomOptions roomOptions);
        void JoinRoom(string roomName);
        void JoinRandomRoom();
        void LeaveRoom();
        void LeaveLobby();
        void Connect();
        void Disconnect();
        void JoinCustonLobby();
        void LoadLevel(string missionSceneName);
        void SetRoomParameters(bool? isOpened = null, bool? isVisible = null);
        int GetRoomPlayersAmount();
        int GetPlayerActorNumber();
        float GetLevelLoadingProgress();
        Dictionary<int, Player> GetRoomPlayers();
    }
}