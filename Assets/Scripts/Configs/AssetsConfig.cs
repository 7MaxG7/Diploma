using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Configs.Data;
using Infrastructure;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Configs
{
    [CreateAssetMenu(fileName = nameof(AssetsConfig), menuName = "Configs/" + nameof(AssetsConfig), order = 8)]
    public class AssetsConfig : ScriptableObject
    {
        [SerializeField] private AssetReferenceGameObject[] _mainMenuAssetReferences;
        [SerializeField] private AssetReferenceGameObject[] _missionAssetReferences;

        
        public AssetReferenceGameObject[] GetAssetReferencesForState(Type type)
        {
            if (type == typeof(MainMenuState))
                return _mainMenuAssetReferences;
            if (type == typeof(LoadMissionState))
                return _missionAssetReferences;
            return Array.Empty<AssetReferenceGameObject>();
        }
    }
}