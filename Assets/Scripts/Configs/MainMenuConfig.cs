using UnityEngine;


namespace Infrastructure {

	[CreateAssetMenu(menuName = "Configs/" + nameof(MainMenuConfig), fileName = nameof(MainMenuConfig), order = 1)]
	internal class MainMenuConfig : ScriptableObject {
		[Header("Credits")]
		[SerializeField] private float _creditsFadingDuration;
		[SerializeField] private float _creditsScrollSpeed;
		[Header("Lobby")]
		[SerializeField] private MainMenuView _mainMenuPref;
		[SerializeField] private LobbyCachedRoomItemView _lobbyCachedRoomItemPref;
		[SerializeField] private RoomPlayerItemView _roomCachedPlayerItemPref;

		public float CreditsFadingDuration => _creditsFadingDuration;
		public float CreditsScrollSpeed => _creditsScrollSpeed;
		public MainMenuView MainMenuPref => _mainMenuPref;
		public LobbyCachedRoomItemView LobbyCachedRoomItemPref => _lobbyCachedRoomItemPref;
		public RoomPlayerItemView RoomCachedPlayerItemPref => _roomCachedPlayerItemPref;
	}

}