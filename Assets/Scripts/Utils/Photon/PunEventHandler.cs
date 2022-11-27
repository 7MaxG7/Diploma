using System;
using System.Collections.Generic;
using Enums.Photon;
using ExitGames.Client.Photon;
using Infrastructure;
using Photon.Pun;
using Services;
using UnityEngine;
using Weapons;
using Zenject;

namespace Utils.Photon
{
    internal class PunEventHandler : IPunEventHandler
    {
        public event Action<PhotonView> OnEnemyObjectCreated;
        public event Action<int, bool> OnObjectActivated;
        public event Action<int, int> OnDamageReceived;

        private readonly IPhotonManager _photonManager;
        private readonly IAmmosFactory _ammosFactory;
        private readonly IUnitsFactory _unitsFactory;


        [Inject]
        public PunEventHandler(IPhotonManager photonManager, IAmmosFactory ammosFactory, IUnitsFactory unitsFactory
            , IControllersHolder controllersHolder)
        {
            _photonManager = photonManager;
            _ammosFactory = ammosFactory;
            _unitsFactory = unitsFactory;
            controllersHolder.AddController(this);
        }

        public void OnEvent(EventData photonEvent)
        {
            if (!IsCustomEvent(photonEvent))
                return;
            
            var data = (object[]) photonEvent.CustomData;
            switch (photonEvent.Code)
            {
                case (byte)PunEventType.PlayerSpawn:
                    HandlePlayerSpawn(data);
                    break;
                case (byte)PunEventType.MonsterCreation:
                    HandleMonsterCreation(data);
                    break;
                case (byte)PunEventType.AmmoCreation:
                    HandleAmmoCreation(data);
                    break;
                case (byte)PunEventType.ObjectActivation:
                    HandleObjectActivation(data);
                    break;
                case (byte)PunEventType.DamageReceiving:
                    HandleDamageReceiving(data);
                    break;
                default:
                    return;
            }
        }

        public void CleanUp()
        {
            _photonManager.UnsubscribeCallbacks(this);
        }

        public void Init()
        {
            _photonManager.SubscribeCallbacks(this);
        }

        private async void HandlePlayerSpawn(IReadOnlyList<object> data)
        {
            var position = (Vector2)data[Constants.CREATION_DATA_POSITION_INDEX];
            var rotation = (Quaternion)data[Constants.CREATION_DATA_ROTATION_INDEX];
            var viewId = (int)data[Constants.CREATION_DATA_VIEW_ID_INDEX];

            var unit = await _unitsFactory.CreatePlayerAsync(position, rotation);
            unit.PhotonView.ViewID = viewId;
            OnEnemyObjectCreated?.Invoke(unit.PhotonView);
        }

        private async void HandleMonsterCreation(IReadOnlyList<object> data)
        {
            var position = (Vector2)data[Constants.CREATION_DATA_POSITION_INDEX];
            var rotation = (Quaternion)data[Constants.CREATION_DATA_ROTATION_INDEX];
            var viewId = (int)data[Constants.CREATION_DATA_VIEW_ID_INDEX];
            var monsterLevel = (int)data[Constants.CREATION_DATA_TYPE_INDEX];

            var unit = await _unitsFactory.CreateMonsterAsync(monsterLevel, position, rotation);
            unit.PhotonView.ViewID = viewId;
            OnEnemyObjectCreated?.Invoke(unit.PhotonView);
        }

        private async void HandleAmmoCreation(IReadOnlyList<object> data)
        {
            var position = (Vector2)data[Constants.CREATION_DATA_POSITION_INDEX];
            var rotation = (Quaternion)data[Constants.CREATION_DATA_ROTATION_INDEX];
            var viewId = (int)data[Constants.CREATION_DATA_VIEW_ID_INDEX];
            var weaponType = (int)data[Constants.CREATION_DATA_TYPE_INDEX];

            var ammo = await _ammosFactory.CreateAmmoAsync((WeaponType)weaponType, position, rotation);
            ammo.PhotonView.ViewID = viewId;
            OnEnemyObjectCreated?.Invoke(ammo.PhotonView);
        }

        private void HandleObjectActivation(IReadOnlyList<object> data)
        {
            var viewId = (int)data[Constants.ACTIVATION_DATA_VIEW_ID_INDEX];
            var isActive =  (bool)data[Constants.ACTIVATION_DATA_TOGGLE_INDEX];
            OnObjectActivated?.Invoke(viewId, isActive);
        }

        private void HandleDamageReceiving(IReadOnlyList<object> data)
        {
            var viewId = (int)data[Constants.DAMAGING_DATA_VIEW_ID_INDEX];
            var damage =  (int)data[Constants.DAMAGING_DATA_VALUE_INDEX];
            OnDamageReceived?.Invoke(viewId, damage);
        }

        private static bool IsCustomEvent(EventData photonEvent)
        {
            var eventCode = (int)photonEvent.Code;
            return Enum.IsDefined(typeof(PunEventType), eventCode);
        }
    }
}