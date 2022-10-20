using System;
using Infrastructure;
using UnityEngine;


namespace Services
{
    internal interface ICameraManager : ILateUpdater, IDisposable
    {
        bool CameraIsPositioned { get; }

        void Follow(Transform target, Vector3 cameraOffset);
    }
}