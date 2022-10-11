using Photon.Pun;
using UnityEngine;

namespace Units
{
    internal interface IUnitView : IDamagableView
    {
        PhotonView PhotonView { get; }
        
        void Locate(Vector2 position);
        void Move(Vector3 deltaPosition);
        void StopMoving();
        void ToggleActivation(bool isActive);
    }
}