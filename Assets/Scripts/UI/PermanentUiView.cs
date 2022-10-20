using System;
using System.Collections;
using System.Threading.Tasks;
using DG.Tweening;
using Infrastructure;
using TMPro;
using UnityEngine;


namespace UI
{
    internal sealed class PermanentUiView : MonoBehaviour, IPermanentUiView
    {
        [SerializeField] private CanvasGroup _loadingCurtainCanvasGroup;
        [SerializeField] private TMP_Text _loadingCurtainText;
        [SerializeField] private SettingsPanelView _settingsPanel;
        [SerializeField] private ResultPanelView _resultPanel;

        public event Action OnCurtainShown;

        public GameObject GameObject => gameObject;
        public SettingsPanelView SettingsPanel => _settingsPanel;
        public ResultPanelView ResultPanel => _resultPanel;
        public bool CurtainIsActive { get; private set; }
        public bool CurtainIsActivating { get; private set; }

        private UiConfig _uiConfig;
        private ICoroutineRunner _coroutineRunner;
        private bool _curtainIsDeactivating;


        public void Init(ICoroutineRunner coroutineRunner, UiConfig uiConfig)
        {
            _coroutineRunner = coroutineRunner;
            _uiConfig = uiConfig;
        }

        public async void ShowLoadingStatusLableAsync()
        {
            var currentSymbolsAmount = _uiConfig.MinSuffixSymbolsAmount;
            while (CurtainIsActive || CurtainIsActivating || _curtainIsDeactivating)
            {
                _loadingCurtainText.text =
                    $"{_uiConfig.LoadingStatusPrefixText}{new string(_uiConfig.LoadingStatusSuffixSymbol, currentSymbolsAmount)}";
                if (++currentSymbolsAmount >= _uiConfig.MaxSuffixSymbolsAmount)
                    currentSymbolsAmount = _uiConfig.MinSuffixSymbolsAmount;
                await Task.Delay(_uiConfig.StatusLableSuffixUpdateDelay);
            }
        }

        public void ShowLoadingCurtain(bool isAnimated)
        {
            if (isAnimated)
            {
                _coroutineRunner.StartCoroutine(ShowLoadingCurtainCoroutine());
            }
            else
            {
                _loadingCurtainCanvasGroup.gameObject.SetActive(true);
                _loadingCurtainCanvasGroup.alpha = 1;
                OnCurtainIsShown();
            }
        }

        public void HideLoadingCurtain(bool animationIsOn)
        {
            if (animationIsOn)
            {
                _coroutineRunner.StartCoroutine(HideLoadingCurtainCoroutine());
            }
            else
            {
                CurtainIsActive = false;
                _loadingCurtainCanvasGroup.alpha = 0;
                _loadingCurtainCanvasGroup.blocksRaycasts = false;
                _loadingCurtainCanvasGroup.gameObject.SetActive(false);
            }
        }

        public void StopLoadingCurtainAnimation()
        {
            if (_curtainIsDeactivating)
            {
                _coroutineRunner.StopCoroutine(HideLoadingCurtainCoroutine());
            }

            if (CurtainIsActivating)
            {
                _coroutineRunner.StopCoroutine(ShowLoadingCurtainCoroutine());
            }

            _loadingCurtainCanvasGroup.DOKill();
            CurtainIsActivating = false;
            _curtainIsDeactivating = false;
        }

        private IEnumerator ShowLoadingCurtainCoroutine()
        {
            while (_curtainIsDeactivating)
            {
                yield return new WaitForEndOfFrame();
            }

            CurtainIsActivating = true;
            _loadingCurtainCanvasGroup.gameObject.SetActive(true);
            while (_loadingCurtainCanvasGroup.alpha < 1)
            {
                _loadingCurtainCanvasGroup.alpha += Math.Min(_uiConfig.CurtainFadingAlfaPerFrameDelta,
                    1 - _loadingCurtainCanvasGroup.alpha);
                yield return new WaitForEndOfFrame();
            }

            CurtainIsActivating = false;
            OnCurtainIsShown();
        }

        private void OnCurtainIsShown()
        {
            CurtainIsActive = true;
            _loadingCurtainCanvasGroup.blocksRaycasts = true;
            OnCurtainShown?.Invoke();
        }

        private IEnumerator HideLoadingCurtainCoroutine()
        {
            while (CurtainIsActivating)
            {
                yield return new WaitForEndOfFrame();
            }

            if (!CurtainIsActive)
                yield break;

            _loadingCurtainCanvasGroup.blocksRaycasts = false;
            CurtainIsActive = false;
            _curtainIsDeactivating = true;
            // DOTween lags on application start so coroutine is better here
            while (_loadingCurtainCanvasGroup.alpha > 0)
            {
                _loadingCurtainCanvasGroup.alpha -= Math.Min(_uiConfig.CurtainFadingAlfaPerFrameDelta,
                    _loadingCurtainCanvasGroup.alpha);
                yield return new WaitForEndOfFrame();
            }

            _loadingCurtainCanvasGroup.gameObject.SetActive(false);
            _curtainIsDeactivating = false;
        }

        public void Dispose()
        {
            _loadingCurtainCanvasGroup.DOKill();
        }
    }
}