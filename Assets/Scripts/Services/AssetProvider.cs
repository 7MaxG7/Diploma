using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Configs;
using Infrastructure;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Services
{
    internal class AssetProvider : IAssetProvider
    {
        private readonly AssetsConfig _assetsConfig;
        private readonly Dictionary<string, AsyncOperationHandle> _loadedAssets = new();
        private readonly List<AsyncOperationHandle> _handles = new();
        private bool _isCleaned;
        
        
        [Inject]
        public AssetProvider(IControllersHolder controllersHolder, AssetsConfig assetsConfig)
        {
            _assetsConfig = assetsConfig;
            controllersHolder.AddController(this);
        }
        
        public void CleanUp()
        {
            if (_isCleaned)
                return;

            _isCleaned = true;
            foreach (var handle in _handles) 
                Addressables.Release(handle);
            _handles.Clear();
            _loadedAssets.Clear();
        }

        public void Init()
        {
            Addressables.InitializeAsync();
        }

        public async void WarmUpForState(Type state)
        {
            var assetReferences = _assetsConfig.GetAssetReferencesForState(state);
            foreach (var reference in assetReferences)
            {
                await LoadAsync<GameObject>(reference);
            }
        }

        public async Task<T> LoadAsync<T>(AssetReference assetReference) where T : class
        {
            _isCleaned = false;
            
            if (_loadedAssets.TryGetValue(assetReference.AssetGUID, out var loadedHandle))
                return loadedHandle.Result as T;
            
            var handle = Addressables.LoadAssetAsync<T>(assetReference);
            handle.Completed += resultHandle =>
            {
                _loadedAssets[assetReference.AssetGUID] = resultHandle;
                _handles.Add(handle);
            };
            return await handle.Task;
        }
    }
}