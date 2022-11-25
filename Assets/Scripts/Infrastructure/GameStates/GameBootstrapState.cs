﻿using System;
using DG.Tweening;
using Services;
using UI;
using Utils;
using Zenject;


namespace Infrastructure
{
    internal sealed class GameBootstrapState : IGameBootstrapState
    {
        public event Action OnStateChange;

        private readonly ISceneLoader _sceneLoader;
        private readonly IPermanentUiController _permanentUiController;
        private readonly IInputService _inputService;


        [Inject]
        public GameBootstrapState(ISceneLoader sceneLoader, IPermanentUiController permanentUiController,
            IInputService inputService)
        {
            _sceneLoader = sceneLoader;
            _permanentUiController = permanentUiController;
            _inputService = inputService;
        }

        public void Enter()
        {
            DOTween.Init();
#if UNITY_EDITOR
            UnityUtils.OnPlayModeExit += StopUtils;
#endif
            _permanentUiController.ShowLoadingCurtain(false);
            _inputService.Init();
            _sceneLoader.LoadScene(Constants.BOOTSTRAP_SCENE_NAME, () => OnStateChange?.Invoke());
        }
        
#if UNITY_EDITOR
        private void StopUtils()
        {
            UnityUtils.OnPlayModeExit -= StopUtils;
            DOTween.KillAll();
        }
#endif

        public void Exit()
        {
        }
    }
}