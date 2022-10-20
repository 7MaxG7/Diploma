using System;
using System.Collections.Generic;
using Infrastructure;
using Units;


namespace Services
{
    internal interface IPlayersInteractionManager : IUpdater, IDisposable
    {
        bool IsPlayersFight { get; }
        PlayerView ClosestFightingEnemyPlayer { get; }
        float ClosestFightingEnemyPlayerSqrMagnitude { get; }
        bool? IsMultiplayerGame { get; }

        void Init(IUnit player, List<PlayerView> enemyPlayerViews);
    }
}