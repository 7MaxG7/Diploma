using System;
using UnityEngine;

namespace Configs.Data
{
    [Serializable]
    internal class StateLabels
    {
        [SerializeField] private Component _state;
        [SerializeField] private string[] _labels;

        public Type State => _state.GetType();
        public string[] Labels => _labels;
    }
}