using System;
using Units;


namespace Infrastructure {

	internal interface ISkillsManager : IDisposable {
		void Init(IUnit player);
	}

}