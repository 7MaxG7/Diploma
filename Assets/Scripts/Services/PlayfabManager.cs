using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using Utils;

// ReSharper disable InconsistentNaming


namespace Services
{
    internal sealed class PlayfabManager : IPlayfabManager
    {
        public void GetData(string playfabId, Action<Dictionary<string, string>> successCallback,
            Action<PlayFabError> errorCallback
            , params string[] keys)
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest
            {
                PlayFabId = playfabId,
                Keys = new List<string>(keys)
            }, OnSuccessResult, errorCallback);


            void OnSuccessResult(GetUserDataResult result)
            {
                var data = new Dictionary<string, string>();
                if (result.Data != null)
                {
                    foreach (var key in keys)
                    {
                        if (result.Data.ContainsKey(key))
                        {
                            data[key] = result.Data[key].Value;
                        }
                    }
                }

                successCallback?.Invoke(data);
            }
        }

        public void SetData(Dictionary<string, string> data, Action successCallback, Action<PlayFabError> errorCallback)
        {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest { Data = data }, OnSuccessResult, OnErrorResult);


            void OnSuccessResult(UpdateUserDataResult result)
            {
                successCallback?.Invoke();
            }

            void OnErrorResult(PlayFabError error)
            {
                errorCallback?.Invoke(error);
            }
        }

        public bool CheckClientAutorization()
        {
            return PlayFabClientAPI.IsClientLoggedIn();
        }

        public void InitTitle(string playfabTitleId)
        {
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            {
                PlayFabSettings.staticSettings.TitleId = playfabTitleId;
            }
        }

        public void RegisterUser(string username, string password, string email,
            Action<RegisterPlayFabUserResult> successCallback
            , Action<PlayFabError> failuerCallback)
        {
            PlayFabClientAPI.RegisterPlayFabUser(
                new RegisterPlayFabUserRequest
                    { Username = username, Password = password, Email = email, RequireBothUsernameAndEmail = true }
                , successCallback
                , failuerCallback
            );
        }

        public void LoginUser(string username, string password, Action<LoginResult> successCallback,
            Action<PlayFabError> failuerCallback)
        {
#if UNITY_EDITOR
            UnityUtils.OnPlayModeExit += PlayFabClientAPI.ForgetAllCredentials;
#endif
            PlayFabClientAPI.LoginWithPlayFab(
                new LoginWithPlayFabRequest { Username = username, Password = password }
                , successCallback
                , failuerCallback
            );
        }
    }
}