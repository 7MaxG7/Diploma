namespace Utils {

	internal static class Constants {
		public const string PLAYFAB_TITLE_ID = "AA95E";
		
		// Map
		public const int GROUND_ITEMS_AMOUNT_PER_PLAYER_ZONE_LENGTH = 10;
		public const int HORIZONTAL_GROUND_ITEMS_AMOUNT = 3;
		public const int VERTICAL_GROUND_ITEMS_AMOUNT = 3;
		/// <summary>
		/// Distance (in ground items) between player and item, when the item is far enough to relocate it
		/// </summary>
		public const float MAX_PLAYER_TO_GROUND_ITEM_DISTANCE_RATE = 1.5f;
		/// <summary>
		/// Distance (in ground items) to relocate an item when the player moved away enough from it
		/// </summary>
		public const float GROUND_ITEM_RELOCATION_DISTANCE_RATE = 3f;
		
		// Scenes
		public const string BOOTSTRAP_SCENE_NAME = "BootstrapScene";
		public const string MAIN_MENU_SCENE_NAME = "MainMenuScene";
		public const string MISSION_SCENE_NAME = "MissionScene";
			
		// Input
		public const string HORIZONTAL = "Horizontal";
		public const string VERTICAL = "Vertical";
			
		// Mission
		public const string GROUND_ITEMS_PARENT_NAME = "[GROUND]";
		public const string UI_ROOT_NAME = "[UI]";
		public const float CHARACTER_SPEED_STOP_TRESHOLD = .001f;
			
		// Settings
		public const string MUSIC_VOLUME_PREFS_KEY = "music_volume";
		public const string SOUND_VOLUME_PREFS_KEY = "sound_value";
		
		// Playfab
		public const string WINS_AMOUNT_PLAYFAB_KEY = "wins_amount";
		public const string KILLS_AMOUNT_PLAYFAB_KEY = "kills_amount";
	}

}