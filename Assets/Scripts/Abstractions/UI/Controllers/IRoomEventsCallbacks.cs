using System;


namespace Abstractions.UI.Controllers {

	internal interface IRoomEventsCallbacks {
		event Action OnLobbyJoin;
		event Action OnRoomCreationFail;
		event Action OnRoomJoinFail;
		event Action OnRandomRoomJoinFail;
	}

}