using UnityEngine;


namespace Infrastructure {

	[CreateAssetMenu(menuName = "Configs/" + nameof(LobbyConfig), fileName = nameof(LobbyConfig), order = 1)]
	internal class LobbyConfig : ScriptableObject {
		[SerializeField] private LobbyCachedRoomItemView _lobbyCachedRoomItemPref;
		[SerializeField] private RoomPlayerItemView _roomCachedPlayerItemPref;

		public LobbyCachedRoomItemView LobbyCachedRoomItemPref => _lobbyCachedRoomItemPref;
		public RoomPlayerItemView RoomCachedPlayerItemPref => _roomCachedPlayerItemPref;
	}

}