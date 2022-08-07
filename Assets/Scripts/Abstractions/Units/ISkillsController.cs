using System;
using Units;


namespace Infrastructure {

	internal interface ISkillsController : IDisposable {
		void Init(IUnit player);
	}

}