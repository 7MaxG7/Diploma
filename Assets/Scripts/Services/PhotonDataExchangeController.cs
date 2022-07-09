using System;
using System.Collections.Generic;
using Enums;
using Photon.Pun;


namespace Services {

	internal class PhotonDataExchangeController : IPhotonDataExchangeController, IDisposable {
		public event Action<int, bool> OnActivationDataRecieved;
		public event Action<int> OnInstantiationDataRecieved;
		
		private readonly List<List<object>> _data = new();

		private PhotonDataExchanger _minePhotonDataExchanger;
		private List<PhotonDataExchanger> _othersPhotonDataExchangers;

		
		public void Dispose() {
			_minePhotonDataExchanger.OnDataWriting -= SendData;
		}

		public void Init(PhotonDataExchanger minePhotonDataExchanger, List<PhotonDataExchanger> othersPhotonDataExchangers) {
			_minePhotonDataExchanger = minePhotonDataExchanger;
			_othersPhotonDataExchangers = othersPhotonDataExchangers;
			
			_minePhotonDataExchanger.OnDataWriting += SendData;
			foreach (var othersPhotonDataExchanger in _othersPhotonDataExchangers) {
				othersPhotonDataExchanger.OnDataReading += RecieveData;
			}
		}

		private void RecieveData(PhotonStream stream) {
			var dataCount = (int)stream.ReceiveNext();
			for (var i = 0; i < dataCount; i++) {
				var dataType = (PhotonExchangerDataType)stream.ReceiveNext();
				switch (dataType) {
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
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		private void SendData(PhotonStream stream) {
			var dataCount = _data.Count;
			stream.SendNext(dataCount);
			for (var i = 0; i < dataCount; i++) {
				foreach (var dataParam in _data[i]) {
					stream.SendNext(dataParam);
				}
			}
			_data.Clear();
		}

		public void PrepareDataForSending(PhotonExchangerDataType dataType, params object[] data) {
			var parameters = new List<object> { dataType };
			parameters.AddRange(data);
			_data.Add(parameters);
		}

	}

}