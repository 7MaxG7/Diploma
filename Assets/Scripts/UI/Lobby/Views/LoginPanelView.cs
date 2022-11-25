using System;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;


namespace Infrastructure
{
    internal sealed class LoginPanelView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_InputField _usernameInputField;
        [SerializeField] private TMP_InputField _passwordInputField;
        [SerializeField] private TMP_InputField _emailInputField;
        [SerializeField] private CanvasGroup _emailInputFieldCanvasGroup;
        [SerializeField] private Toggle _emailToggleSwitcher;
        [SerializeField] private CanvasGroup _emailToggleSwitcherLabelCanvasGroup;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _closePanelButton;
        [SerializeField] private TMP_Text _confirmButtonText;
        [SerializeField] private TMP_Text _statusLableText;

        public event Action<string, string, string> OnLoginConfirmClick;
        public event Action OnHideLoginPanelClick;
        public event Action<bool> OnLoginModeSwitched;

        private MainMenuConfig _mainMenuConfig;
        private bool _isNewAccount;
        private bool _loadingStatusIsOn;


        public void Init(MainMenuConfig mainMenuConfig)
        {
            _mainMenuConfig = mainMenuConfig;
            _usernameInputField.onValueChanged.AddListener(_ => UpdateConfirmButtonInteractivity());
            _passwordInputField.onValueChanged.AddListener(_ => UpdateConfirmButtonInteractivity());
            _emailInputField.onValueChanged.AddListener(_ => UpdateConfirmButtonInteractivity());
            _confirmButton.onClick.AddListener(() =>
                OnLoginConfirmClick?.Invoke(_usernameInputField.text, _passwordInputField.text, _emailInputField.text));
            _closePanelButton.onClick.AddListener(() => OnHideLoginPanelClick?.Invoke());
            _confirmButtonText.text = _mainMenuConfig.ConfirmLoginAccountButtonText;
            _statusLableText.text = string.Empty;
#if UNITY_EDITOR
            _usernameInputField.text = Constants.DEFAULT_EDITOR_USERNAME;
            _passwordInputField.text = Constants.DEFAULT_EDITOR_PASSWORD;
#endif
            InitEmailToggleSwitcher();
            UpdateConfirmButtonInteractivity();

            void InitEmailToggleSwitcher()
            {
                _emailToggleSwitcher.isOn = false;
                HideEmailField(false);
                _emailToggleSwitcherLabelCanvasGroup.gameObject.SetActive(true);
                _emailToggleSwitcherLabelCanvasGroup.alpha = 1;
                _emailToggleSwitcher.onValueChanged.AddListener(ToggleLoginMode);
            }
        }

        public void OnDispose()
        {
            _canvasGroup.DOKill();
            _usernameInputField.onValueChanged.RemoveAllListeners();
            _passwordInputField.onValueChanged.RemoveAllListeners();
            _emailInputField.onValueChanged.RemoveAllListeners();
            _confirmButton.onClick.RemoveAllListeners();
            _closePanelButton.onClick.RemoveAllListeners();
            _emailToggleSwitcher.onValueChanged.RemoveAllListeners();
        }

        public void Show()
        {
            _canvasGroup.DOKill();
            gameObject.SetActive(true);
            _canvasGroup.alpha = 0;
            _canvasGroup.DOFade(1, _mainMenuConfig.LoginPanelFadingDuration);
        }

        public void Hide()
        {
            _canvasGroup.DOKill();
            _canvasGroup.DOFade(0, _mainMenuConfig.LoginPanelFadingDuration)
                .OnComplete(() => gameObject.SetActive(false));
        }

        public async void ShowLoadingStatusAsync()
        {
            _loadingStatusIsOn = true;
            _statusLableText.color = _mainMenuConfig.OrdinaryStatusTextColor;
            var currentAwaitingTick = _mainMenuConfig.MinStatusSuffixSymbolsAmount;
            while (_loadingStatusIsOn)
            {
                _statusLableText.text =
                    $"{_mainMenuConfig.LoadingStatusPrefixText}{new string(_mainMenuConfig.LoadingStatusSuffixSymbol, currentAwaitingTick)}";
                if (++currentAwaitingTick >= _mainMenuConfig.MaxStatusSuffixSymbolsAmount)
                    currentAwaitingTick = _mainMenuConfig.MinStatusSuffixSymbolsAmount;
                await Task.Delay(_mainMenuConfig.StatusLableSuffixUpdateDelay);
            }
        }

        public void ShowRegistrationSuccessStatus()
        {
            _loadingStatusIsOn = false;
            _statusLableText.color = _mainMenuConfig.SuccessStatusTextColor;
            _statusLableText.text = _mainMenuConfig.RegistrationSuccessStatusText;
        }

        public void ShowRegistrationErrorStatus()
        {
            _loadingStatusIsOn = false;
            _statusLableText.color = _mainMenuConfig.ErrorStatusTextColor;
            _statusLableText.text = _mainMenuConfig.RegistrationFailStatusText;
        }

        public void ShowLoginSuccessStatus()
        {
            _loadingStatusIsOn = false;
            _statusLableText.color = _mainMenuConfig.SuccessStatusTextColor;
            _statusLableText.text = _mainMenuConfig.LoginSuccessStatusText;
        }

        public void ShowLoginErrorStatus()
        {
            _loadingStatusIsOn = false;
            _statusLableText.color = _mainMenuConfig.ErrorStatusTextColor;
            _statusLableText.text = _mainMenuConfig.LoginFailStatusText;
        }

        public void ToggleElementsInteractivity(bool isInteratable)
        {
            _usernameInputField.interactable = isInteratable;
            _passwordInputField.interactable = isInteratable;
            _emailInputField.interactable = isInteratable;
            _emailToggleSwitcher.interactable = isInteratable;
            _confirmButton.interactable = isInteratable;
            _closePanelButton.interactable = isInteratable;
        }

        private void ToggleLoginMode(bool isNewAccount)
        {
            _isNewAccount = isNewAccount;
            _emailToggleSwitcher.interactable = false;
            OnLoginModeSwitched?.Invoke(isNewAccount);
            UpdateConfirmButtonInteractivity();
            if (isNewAccount)
            {
                _confirmButtonText.text = _mainMenuConfig.ConfirmCreateAccountButtonText;
                _emailToggleSwitcherLabelCanvasGroup.DOFade(0, _mainMenuConfig.LoginPanelFadingDuration)
                    .OnComplete(() =>
                    {
                        _emailToggleSwitcherLabelCanvasGroup.gameObject.SetActive(false);
                        ShowEmailField();
                    });
            }
            else
            {
                _confirmButtonText.text = _mainMenuConfig.ConfirmLoginAccountButtonText;
                HideEmailField(onCompleteCallback: () =>
                {
                    _emailToggleSwitcherLabelCanvasGroup.gameObject.SetActive(true);
                    _emailToggleSwitcherLabelCanvasGroup.DOFade(1, _mainMenuConfig.LoginPanelFadingDuration);
                    _emailToggleSwitcher.interactable = true;
                });
            }

            void ShowEmailField()
            {
                _emailInputFieldCanvasGroup.gameObject.SetActive(true);
                _emailInputFieldCanvasGroup.DOFade(1, _mainMenuConfig.LoginPanelFadingDuration)
                    .OnComplete(() => _emailToggleSwitcher.interactable = true);
            }
        }

        private void HideEmailField(bool animationIsOn = true, Action onCompleteCallback = null)
        {
            if (animationIsOn)
                _emailInputFieldCanvasGroup.DOFade(0, _mainMenuConfig.LoginPanelFadingDuration)
                    .OnComplete(() =>
                    {
                        _emailInputFieldCanvasGroup.gameObject.SetActive(false);
                        onCompleteCallback?.Invoke();
                    });
            else
            {
                _emailInputFieldCanvasGroup.gameObject.SetActive(false);
                onCompleteCallback?.Invoke();
            }
        }

        private void UpdateConfirmButtonInteractivity()
        {
            _confirmButton.interactable = !string.IsNullOrEmpty(_usernameInputField.text)
                                          && !string.IsNullOrEmpty(_passwordInputField.text)
                                          && (_isNewAccount && !string.IsNullOrEmpty(_emailInputField.text) ||
                                              !_isNewAccount);
        }
    }
}