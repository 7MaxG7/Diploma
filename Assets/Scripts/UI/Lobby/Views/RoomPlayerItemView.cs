using TMPro;
using UnityEngine;


namespace Infrastructure {

	internal sealed class RoomPlayerItemView : MonoBehaviour {
		[SerializeField] private TMP_Text _playerName;
		
		public TMP_Text PlayerName => _playerName;
	}

}