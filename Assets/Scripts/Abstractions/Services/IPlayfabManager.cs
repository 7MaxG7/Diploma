using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;


namespace Services
{
    internal interface IPlayfabManager
    {
        void GetData(string playfabId, Action<Dictionary<string, string>> successCallback,
            Action<PlayFabError> errorCallback
            , params string[] keys);

        void SetData(Dictionary<string, string> data, Action successCallback, Action<PlayFabError> errorCallback);
        bool CheckClientAutorization();
        void InitTitle(string playfabTitleId);

        void RegisterUser(string username, string password, string email,
            Action<RegisterPlayFabUserResult> successCallback
            , Action<PlayFabError> failuerCallback);

        void LoginUser(string username, string password, Action<LoginResult> successCallback,
            Action<PlayFabError> failuerCallback);
    }
}