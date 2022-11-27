using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure;
using Units;
using Utils;


namespace Services
{
    internal interface IPlayersInteractionManager : IUpdater, IDisposable
    {
        bool IsPlayersFight { get; }
        PlayerView ClosestFightingEnemyPlayer { get; }
        float ClosestFightingEnemyPlayerSqrMagnitude { get; }
        bool? IsMultiplayerGame { get; }

        void Init(IUnit player);
        Task PrepareOtherPlayers(IUnitsFactory unitsFactory);
    }
}