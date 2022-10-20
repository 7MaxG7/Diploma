using System;
using Photon.Pun;


namespace Services
{
    internal sealed class PhotonDataExchanger : MonoBehaviourPunCallbacks, IPunObservable
    {
        public event Action<PhotonStream> OnDataWriting;
        public event Action<PhotonStream> OnDataReading;

        #region IPunObservableMethods

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                OnDataWriting?.Invoke(stream);
            }
            else
            {
                OnDataReading?.Invoke(stream);
            }
        }

        #endregion
    }
}