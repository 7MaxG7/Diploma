using System;
using System.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using Services;
using UnityEngine;
using Utils;

// ReSharper disable InconsistentNaming


namespace Infrastructure
{
    internal sealed class LoginPanelController : IDisposable
    {
        public event Action<string, string> OnUserLoginSuccess;

        private readonly IPlayfabManager _playfabManager;
        private readonly LoginPanelView _loginPanelView;
        private bool _isNewAccount;
        private bool _isFading;
        private bool _emailFieldIsFading;
        private bool _isLoading;

        private string _username;
        private string _password;
        private string _email;
        private readonly MainMenuConfig _mainMenuConfig;


        public LoginPanelController(IPlayfabManager playfabManager, LoginPanelView loginPanelView,
            MainMenuConfig mainMenuConfig)
        {
            _playfabManager = playfabManager;
            _loginPanelView = loginPanelView;
            _mainMenuConfig = mainMenuConfig;
        }

        public void Dispose()
        {
            OnDispose();
        }

        public void OnDispose()
        {
            _loginPanelView.OnDispose();
            _loginPanelView.OnLoginConfirmClick -= ConfirmLogin;
            _loginPanelView.OnHideLoginPanelClick -= _loginPanelView.Hide;
            _loginPanelView.OnLoginModeSwitched -= ToggleLoginMode;
        }

        public void Init()
        {
            _loginPanelView.Init(_mainMenuConfig);
            _loginPanelView.OnLoginConfirmClick += ConfirmLogin;
            _loginPanelView.OnHideLoginPanelClick += _loginPanelView.Hide;
            _loginPanelView.OnLoginModeSwitched += ToggleLoginMode;
            _playfabManager.InitTitle(Constants.PLAYFAB_TITLE_ID);
        }

        public void ShowPanel()
        {
            _loginPanelView.Show();
        }

        /// <summary>
        /// Switching between login to existing account and creating account modes
        /// </summary>
        private void ToggleLoginMode(bool isNewAccount)
        {
            _isNewAccount = isNewAccount;
        }

        private void ConfirmLogin(string userName, string password, string email)
        {
            _username = userName;
            _password = password;
            _email = email;
            if (_isNewAccount)
                CreateAccount();
            else
                LoginAccount();
        }

        private void CreateAccount()
        {
            _loginPanelView.ToggleElementsInteractivity(false);
            _loginPanelView.ShowLoadingStatusAsync();
            _playfabManager.RegisterUser(_username, _password, _email, OnRegistrationSuccess, OnRegistrationFailure);


            void OnRegistrationSuccess(RegisterPlayFabUserResult _)
            {
                Debug.Log($"Registration success: {_username}");
                LoginAccount(true);
            }

            void OnRegistrationFailure(PlayFabError playFabError)
            {
                _loginPanelView.ToggleElementsInteractivity(true);
                _loginPanelView.ShowRegistrationErrorStatus();
                Debug.LogError($"Registration fail: {playFabError.ErrorMessage}");
            }
        }

        private void LoginAccount(bool isAfterRegistrationLogin = false)
        {
            if (!isAfterRegistrationLogin)
            {
                _loginPanelView.ToggleElementsInteractivity(false);
                _loginPanelView.ShowLoadingStatusAsync();
            }

            _playfabManager.LoginUser(_username, _password, OnLoginSuccessAsync, OnLoginFailure);


            async void OnLoginSuccessAsync(LoginResult loginPlayFabUserResult)
            {
                _loginPanelView.ShowLoginSuccessStatus();
                await Task.Delay(_mainMenuConfig.SuccessfulStatusDelay);
                OnUserLoginSuccess?.Invoke(_username, loginPlayFabUserResult.PlayFabId);
                _loginPanelView.Hide();
                Debug.Log($"Login success: {_username}");
            }

            void OnLoginFailure(PlayFabError playFabError)
            {
                _loginPanelView.ToggleElementsInteractivity(true);
                if (isAfterRegistrationLogin)
                {
                    _loginPanelView.ShowRegistrationSuccessStatus();
                }
                else
                {
                    _loginPanelView.ShowLoginErrorStatus();
                }

                Debug.LogError($"Login fail: {playFabError.ErrorMessage}");
            }
        }
    }
}