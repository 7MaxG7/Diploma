using System;


namespace Units {

	internal interface ISkillsManager : IDisposable {
		void Init(IUnit player);
	}

}