using System.Linq;
using UnityEngine;


namespace Units
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(PlayerConfig), fileName = nameof(PlayerConfig), order = 4)]
    internal class PlayerConfig : ScriptableObject
    {
        [SerializeField] private string _playerPrefabPath;
        [SerializeField] private float _baseMoveSpeed;
        [SerializeField] private LevelExperienceParam[] _levelExpParameters;
        [SerializeField] private LevelHealthParam[] _levelHpParameters;

        public float BaseMoveSpeed => _baseMoveSpeed;
        public LevelExperienceParam[] LevelExpParameters => _levelExpParameters.OrderBy(item => item.Level).ToArray();
        public LevelHealthParam[] LevelHpParameters => _levelHpParameters.OrderBy(item => item.Level).ToArray();
        public string PlayerPrefabPath => _playerPrefabPath;
    }
}