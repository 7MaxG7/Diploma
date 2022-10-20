using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Infrastructure
{
    internal sealed class LobbyCachedRoomItemView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _roomName;
        [SerializeField] private Button _roomButton;

        public TMP_Text RoomName => _roomName;
        public Button RoomButton => _roomButton;
    }
}