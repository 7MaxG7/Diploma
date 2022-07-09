using System.Collections.Generic;


namespace Infrastructure {

	internal class ControllersHolder : /*IAwaker, IStarter,*/ IControllersHolder {
		// private readonly List<IAwaker> _awakers = new();
		// private readonly List<IStarter> _starters = new();
		private readonly List<IUpdater> _updaters = new();
		private readonly List<ILateUpdater> _lateUpdaters = new();
		private readonly List<IFixedUpdater> _fixedUpdaters = new();
		private readonly List<IDisposer> _disposers = new();

		public void AddController(IController controller) {
			// if (controller is IAwaker awaker)
			// 	_awakers.Add(awaker);
			// if (controller is IStarter starter)
			// 	_starters.Add(starter);
			if (controller is IUpdater updater)
				_updaters.Add(updater);
			if (controller is ILateUpdater lateUpdater)
				_lateUpdaters.Add(lateUpdater);
			if (controller is IFixedUpdater fixedUpdater)
				_fixedUpdaters.Add(fixedUpdater);
			if (controller is IDisposer disposer)
				_disposers.Add(disposer);
		}

		public void ClearControllers() {
			OnDispose();
			// _awakers.Clear();
			// _starters.Clear();
			_updaters.Clear();
			_lateUpdaters.Clear();
			_fixedUpdaters.Clear();
			_disposers.Clear();
		}


		// public void OnAwake() {
		// 	foreach (var awaker in _awakers) {
		// 		awaker.OnAwake();
		// 	}
		// }
		//
		// public void OnStart() {
		// 	foreach (var starter in _starters) {
		// 		starter.OnStart();
		// 	}
		// }

		public void OnUpdate(float deltaTime) {
			foreach (var updater in _updaters) {
				updater.OnUpdate(deltaTime);
			}
		}

		public void OnLateUpdate(float deltaTime) {
			foreach (var lateUpdater in _lateUpdaters) {
				lateUpdater.OnLateUpdate(deltaTime);
			}
		}

		public void OnFixedUpdate(float deltaTime) {
			foreach (var fixedUpdater in _fixedUpdaters) {
				fixedUpdater.OnFixedUpdate(deltaTime);
			}
		}

		public void OnDispose() {
			foreach (var disposer in _disposers) {
				disposer.OnDispose();
			}
		}
	}

}