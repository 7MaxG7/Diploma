using System;
using System.Collections.Generic;
using Enums;
using Photon.Pun;
using Zenject;


namespace Services
{
    internal sealed class PhotonDataExchangeController : IPhotonDataExchangeController
    {
        private readonly IPhotonManager _photonManager;
        public event Action<int, bool> OnActivationDataRecieved;
        public event Action<int> OnInstantiationDataRecieved;
        public event Action<int, int> OnDamagePlayerDataRecieved;

        private readonly Queue<List<object>> _data = new();
        private PhotonDataExchanger _minePhotonDataExchanger;
        private List<PhotonDataExchanger> _othersPhotonDataExchangers;


        [Inject]
        public PhotonDataExchangeController(IPhotonManager photonManager)
        {
            _photonManager = photonManager;
        }

        public void Dispose()
        {
            _minePhotonDataExchanger.OnDataWriting -= SendData;
            foreach (var othersPhotonDataExchanger in _othersPhotonDataExchangers)
            {
                othersPhotonDataExchanger.OnDataReading -= RecieveData;
            }

            _othersPhotonDataExchangers.Clear();
            _photonManager.Destroy(_minePhotonDataExchanger.photonView);
        }

        public void Init(PhotonDataExchanger minePhotonDataExchanger,
            List<PhotonDataExchanger> othersPhotonDataExchangers)
        {
            _minePhotonDataExchanger = minePhotonDataExchanger;
            _othersPhotonDataExchangers = othersPhotonDataExchangers;

            _minePhotonDataExchanger.OnDataWriting += SendData;
            foreach (var othersPhotonDataExchanger in _othersPhotonDataExchangers)
            {
                othersPhotonDataExchanger.OnDataReading += RecieveData;
            }
        }

        public void PrepareDataForSending(PhotonExchangerDataType dataType, params object[] data)
        {
            var parameters = new List<object> { dataType };
            parameters.AddRange(data);
            _data.Enqueue(parameters);
        }

        private void RecieveData(PhotonStream stream)
        {
            var dataCount = (int)stream.ReceiveNext();
            for (var i = 0; i < dataCount; i++)
            {
                var dataType = (PhotonExchangerDataType)stream.ReceiveNext();
                switch (dataType)
                {
                    case PhotonExchangerDataType.None:
                        break;
                    case PhotonExchangerDataType.ObjectInstantiation:
                        var newPhotonViewId = (int)stream.ReceiveNext();
                        OnInstantiationDataRecieved?.Invoke(newPhotonViewId);
                        break;
                    case PhotonExchangerDataType.ObjectActivation:
                        var photonViewId = (int)stream.ReceiveNext();
                        var isActive = (bool)stream.ReceiveNext();
                        OnActivationDataRecieved?.Invoke(photonViewId, isActive);
                        break;
                    case PhotonExchangerDataType.DamagingEnemyHero:
                        var playerphotonViewId = (int)stream.ReceiveNext();
                        var damage = (int)stream.ReceiveNext();
                        OnDamagePlayerDataRecieved?.Invoke(playerphotonViewId, damage);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void SendData(PhotonStream stream)
        {
            stream.SendNext(_data.Count);
            while (_data.Count > 0)
            {
                var data = _data.Dequeue();
                foreach (var dataParam in data)
                {
                    stream.SendNext(dataParam);
                }
            }
        }
    }
}