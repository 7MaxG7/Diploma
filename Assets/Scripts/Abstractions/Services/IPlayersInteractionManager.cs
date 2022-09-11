using System;
using System.Collections.Generic;
using Infrastructure;
using Units;


namespace Abstractions {

	internal interface IPlayersInteractionManager : IUpdater, IDisposable {
		void Init(IUnit player, List<PlayerView> enemyPlayerViews);

		bool IsPlayersFight { get; }
		PlayerView ClosestFightingEnemyPlayer { get; }
		float ClosestFightingEnemyPlayerSqrMagnitude { get; }
		bool? IsMultiplayerGame { get; }
	}

}