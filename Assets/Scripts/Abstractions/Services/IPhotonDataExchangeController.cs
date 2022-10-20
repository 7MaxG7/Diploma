using System;
using System.Collections.Generic;
using Enums;


namespace Services
{
    internal interface IPhotonDataExchangeController : IDisposable
    {
        event Action<int, bool> OnActivationDataRecieved;
        event Action<int> OnInstantiationDataRecieved;
        event Action<int, int> OnDamagePlayerDataRecieved;

        void Init(PhotonDataExchanger minePhotonDataExchanger, List<PhotonDataExchanger> othersPhotonDataExchangers);
        void PrepareDataForSending(PhotonExchangerDataType dataType, params object[] data);
    }
}