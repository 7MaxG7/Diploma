using System;


namespace Utils
{
    internal sealed class Smoother
    {
        public event Action<float> OnValueUpdateCallback;

        private float _targetValue;
        private float _currentValue;
        private float _deltaValue;
        private readonly int _smoothChangingDurationInFrames;
        private bool _isEnabled;


        public Smoother(int smoothChangingDurationInFrames)
        {
            _smoothChangingDurationInFrames = smoothChangingDurationInFrames;
        }

        public void SetStartValue(float startValue)
        {
            _currentValue = startValue;
        }

        public void SetTargetValue(float targetValue)
        {
            _targetValue = targetValue;
            _deltaValue = (_targetValue - _currentValue) / _smoothChangingDurationInFrames;
            _isEnabled = true;
        }

        public void SmoothUpdate()
        {
            if (!_isEnabled)
                return;

            if (Math.Abs(_targetValue - _currentValue) <= Math.Abs(_deltaValue))
            {
                _currentValue = _targetValue;
                _isEnabled = false;
            }
            else
            {
                _currentValue += _deltaValue;
            }

            OnValueUpdateCallback?.Invoke(_currentValue);
        }
    }
}