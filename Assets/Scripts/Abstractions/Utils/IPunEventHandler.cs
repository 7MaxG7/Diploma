using System;
using Infrastructure;
using Photon.Pun;
using Photon.Realtime;

namespace Utils
{
    internal interface IPunEventHandler : IOnEventCallback, ICleaner
    {
        event Action<PhotonView> OnEnemyObjectCreated;
        event Action<int, bool> OnObjectActivated;
        event Action<int, int> OnDamageReceived;
        
        void Init();
    }
}