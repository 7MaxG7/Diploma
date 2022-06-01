using Infrastructure;
using UnityEngine;


namespace Controllers {

	internal interface IMoveController : IUpdater {
		void Init(CharacterController characterController);
	}

}