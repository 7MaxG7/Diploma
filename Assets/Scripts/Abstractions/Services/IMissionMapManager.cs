using System;
using System.Collections.Generic;
using Infrastructure;
using UnityEngine;


namespace Services {

	internal interface IMissionMapManager : IUpdater, IDisposable {
		void Init(Transform playerTransform, Vector2 groundItemSize, out IEnumerable<IView> groundItems);
	}

}