using Photon.Pun;
using UnityEngine;

namespace Utils
{
    internal interface IPunEventRaiser
    {
        void RaiseAmmoCreation(PhotonView photonView, Vector2 position, Quaternion rotation, int weaponType);
        void RaiseMonsterCreation(PhotonView photonView, Vector2 position, Quaternion rotation, int monsterLevel);
        void RaisePlayerSpawn(PhotonView playerPhotonView, Vector3 transformPosition, Quaternion transformRotation);
        void RaiseObjectActivation(int viewId, bool isActive);
        void RaiseDamageReceiving(int viewID, int damage);
    }
}