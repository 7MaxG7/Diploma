﻿namespace Services {

	internal class PhotonDamageInfo {
		public int PhotonViewID { get; }
		public int Damage { get; }

		
		public PhotonDamageInfo(int photonViewID, int damage) {
			PhotonViewID = photonViewID;
			Damage = damage;
		}
	}

}