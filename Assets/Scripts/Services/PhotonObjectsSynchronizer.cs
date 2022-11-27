using System.Collections.Generic;
using Photon.Pun;
using Units;
using Utils;
using Zenject;


namespace Services
{
    internal sealed class PhotonObjectsSynchronizer : IPhotonObjectsSynchronizer
    {
        private readonly IPunEventHandler _punEventHandler;
        private IUnitView _playerView;
        private readonly Dictionary<int, PhotonView> _otherPlayersObjects = new();


        [Inject]
        public PhotonObjectsSynchronizer(IPunEventHandler punEventHandler)
        {
            _punEventHandler = punEventHandler;
        }

        public void Dispose()
        {
            _punEventHandler.OnEnemyObjectCreated -= Register;
            _punEventHandler.OnObjectActivated -= SetActive;
            _punEventHandler.OnDamageReceived -= DamagePlayer;
            _otherPlayersObjects.Clear();
        }

        public void Init(IUnitView playerView)
        {
            _playerView = playerView;
            _punEventHandler.OnEnemyObjectCreated += Register;
            _punEventHandler.OnObjectActivated += SetActive;
            _punEventHandler.OnDamageReceived += DamagePlayer;
        }


        private void Register(PhotonView photonView)
        {
            _otherPlayersObjects[photonView.ViewID] = photonView;
        }

        private void SetActive(int photonViewId, bool isActive)
        {
            if (_otherPlayersObjects.TryGetValue(photonViewId, out var photonView))
                photonView.gameObject.SetActive(isActive);
        }

        private void DamagePlayer(int playerPhotonId, int damage)
        {
            if (_playerView.PhotonView.ViewID == playerPhotonId)
            {
                _playerView.TakeDamage(damage, null);
            }
        }
    }
}