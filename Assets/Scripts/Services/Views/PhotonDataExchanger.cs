using System;
using System.Collections.Generic;
using Enums;
using Photon.Pun;


namespace Services {

	internal class PhotonDataExchanger : MonoBehaviourPunCallbacks, IPunObservable {
		private readonly List<List<object>> _data = new();
		public event Action<int, bool> OnActivationDataRecieved;
		public event Action<int> OnInstantiationDataRecieved;


#region IPunObservableMethods
		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
			if (stream.IsWriting) {
				var dataCount = _data.Count;
				stream.SendNext(dataCount);
				for (var i = 0; i < dataCount; i++) {
					foreach (var dataParam in _data[i]) {
						stream.SendNext(dataParam);
					}
				}
				_data.Clear();
			} else {
				var dataCount = (int)stream.ReceiveNext();
				for (var i = 0; i < dataCount; i++) {
					var dataType = (PhotonSynchronizerDataType)stream.ReceiveNext();
					switch (dataType) {
						case PhotonSynchronizerDataType.None:
							break;
						case PhotonSynchronizerDataType.ObjectInstantiation:
							var newPhotonViewId = (int)stream.ReceiveNext();
							OnInstantiationDataRecieved?.Invoke(newPhotonViewId);
							break;
						case PhotonSynchronizerDataType.ObjectActivation:
							var photonViewId = (int)stream.ReceiveNext();
							var isActive = (bool)stream.ReceiveNext();
							OnActivationDataRecieved?.Invoke(photonViewId, isActive);
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}
			}
		}
#endregion

		public void SendData(PhotonSynchronizerDataType dataType, params object[] data) {
			var parameters = new List<object> { dataType };
			parameters.AddRange(data);
			_data.Add(parameters);
		}
	}

}