using System;
using System.Threading.Tasks;
using Infrastructure;
using UnityEngine.AddressableAssets;

namespace Services
{
    internal interface IAssetProvider : ICleaner
    {
        void WarmUpForState(Type state);
        void Init();
        Task<T> LoadAsync<T>(AssetReference mainMenuPref) where T : class;
    }
}