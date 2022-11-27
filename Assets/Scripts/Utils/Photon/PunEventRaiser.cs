using System.Linq;
using Enums.Photon;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Services;
using UnityEngine;
using Zenject;

namespace Utils.Photon
{
    internal class PunEventRaiser : IPunEventRaiser
    {
        private readonly IPhotonManager _photonManager;

        
        [Inject]
        public PunEventRaiser(IPhotonManager photonManager)
        {
            _photonManager = photonManager;
        }

        public void RaisePlayerSpawn(PhotonView photonView, Vector3 position, Quaternion rotation)
        {
            var data = GenerateTransformUntypedData(position, rotation);
            if (TryPreapareCreationEvent(photonView, data, out var raiseEventOptions, out var sendOptions))
            {
                PhotonNetwork.RaiseEvent((byte)PunEventType.PlayerSpawn, data, raiseEventOptions, sendOptions);
            }
            else
            {
                Debug.LogError($"{this}: player photon view id allocation fail");
            }
        }

        public void RaiseMonsterCreation(PhotonView photonView, Vector2 position, Quaternion rotation, int monsterLevel)
        {
            var data = GenerateTransformTypedData(position, rotation, monsterLevel);
            if (TryPreapareCreationEvent(photonView, data, out var raiseEventOptions, out var sendOptions))
            {
                PhotonNetwork.RaiseEvent((byte)PunEventType.MonsterCreation, data, raiseEventOptions, sendOptions);
            }
            else
            {
                Debug.LogError($"{this}: ammo photon view id allocation fail");
            }
        }

        public void RaiseAmmoCreation(PhotonView photonView, Vector2 position, Quaternion rotation, int weaponType)
        {
            var data = GenerateTransformTypedData(position, rotation, weaponType);
            if (TryPreapareCreationEvent(photonView, data, out var raiseEventOptions, out var sendOptions))
            {
                PhotonNetwork.RaiseEvent((byte)PunEventType.AmmoCreation, data.ToArray(), raiseEventOptions, sendOptions);
            }
            else
            {
                Debug.LogError($"{this}: ammo photon view id allocation fail");
            }
        }

        public void RaiseObjectActivation(int viewId, bool isActive)
        {
            var data = new object[] { viewId, isActive };
            PrepareRaiseOptions(out var raiseEventOptions, out var sendOptions);
            PhotonNetwork.RaiseEvent((byte)PunEventType.ObjectActivation, data, raiseEventOptions, sendOptions);
        }

        public void RaiseDamageReceiving(int viewID, int damage)
        {
            var data = new object[] { viewID, damage };
            PrepareRaiseOptions(out var raiseEventOptions, out var sendOptions);
            PhotonNetwork.RaiseEvent((byte)PunEventType.DamageReceiving, data, raiseEventOptions, sendOptions);
        }

        private bool TryPreapareCreationEvent(PhotonView photonView, object[] data, out RaiseEventOptions raiseEventOptions, out SendOptions sendOptions)
        {
            if (_photonManager.AllocateViewID(photonView))
            {
                data[Constants.CREATION_DATA_VIEW_ID_INDEX] = photonView.ViewID;
                PrepareRaiseOptions(out raiseEventOptions, out sendOptions);
                return true;
            }

            raiseEventOptions = null;
            sendOptions = default;
            return false;
        }

        private static object[] GenerateTransformTypedData(Vector2 position, Quaternion rotation, int type)
        {
            var data = GenerateTransformUntypedData(position, rotation);
            data[Constants.CREATION_DATA_TYPE_INDEX] = type;
            return data;
        }

        private static object[] GenerateTransformUntypedData(Vector2 position, Quaternion rotation)
        {
            var data = new object[]
            {
                position, rotation, null, null
            };
            return data;
        }

        private static void PrepareRaiseOptions(out RaiseEventOptions raiseEventOptions, out SendOptions sendOptions)
        {
            raiseEventOptions = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others,
                CachingOption = EventCaching.AddToRoomCache
            };
            sendOptions = new SendOptions
            {
                Reliability = true
            };
        }
    }
}