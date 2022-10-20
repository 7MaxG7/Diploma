using System;
using System.Collections.Generic;
using Services;
using UnityEngine;


namespace Infrastructure
{
    internal interface IMapWrapper : IUpdater, IDisposable
    {
        void Init(Vector2 bottomLeftCornerPosition, Vector2 topRightCornerPosition);
        void SetCheckingTransform(Transform checkingTransform);
        void AddDependingTransforms(IEnumerable<IView> dependingTransforms);
    }
}