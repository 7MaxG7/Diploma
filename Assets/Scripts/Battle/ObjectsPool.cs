using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Utils;


namespace Services
{
    internal abstract class ObjectsPool<T> : IDisposable where T : IPoolObject
    {
        protected IPhotonManager PhotonManager;
        protected IPunEventRaiser PunEventRaiser;

        protected readonly Dictionary<int, Stack<T>> Objects = new();
        protected readonly List<T> SpawnedObjects = new();


        public virtual void Dispose()
        {
            foreach (var obj in Objects.Values.SelectMany(obj => obj))
            {
                obj.OnDispose -= PhotonManager.Destroy;
            }

            foreach (var obj in SpawnedObjects)
            {
                obj.OnDispose -= PhotonManager.Destroy;
            }
        }

        public async Task<T> SpawnObjectAsync(Vector2 spawnPosition, Quaternion rotation, params object[] parameters)
        {
            T obj;
            var poolIndex = GetSpecifiedPoolIndex(parameters);
            if (!Objects.ContainsKey(poolIndex))
                Objects.Add(poolIndex, new Stack<T>());

            var objects = Objects[poolIndex];
            if (objects.Count == 0)
            {
                obj = await SpawnSpecifiedObjectAsync(spawnPosition, rotation, parameters);
                obj.OnDispose += PhotonManager.Destroy;
            }
            else
            {
                obj = objects.Pop();
                obj.Respawn(spawnPosition);
            }

            SpawnedObjects.Add(obj);
            TogglePoolObjectActivation(obj, true);
            return obj;
        }

        protected void ReturnObject(T obj)
        {
            SpawnedObjects.Remove(obj);
            TogglePoolObjectActivation(obj, false);
            obj.StopObj();
            obj.Transform.position = Vector3.zero;
            Objects[obj.PoolIndex].Push(obj);
        }

        private void TogglePoolObjectActivation(T obj, bool isActive)
        {
            obj.ToggleActivation(isActive);
            PunEventRaiser.RaiseObjectActivation(obj.PhotonView.ViewID, isActive);
        }

        protected abstract int GetSpecifiedPoolIndex(object[] parameters);

        protected abstract Task<T> SpawnSpecifiedObjectAsync(Vector2 position, Quaternion rotation, object[] parameters);
    }
}