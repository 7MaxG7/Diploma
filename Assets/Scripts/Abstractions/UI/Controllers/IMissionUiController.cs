﻿using System;
using Infrastructure;
using Units;


namespace UI {

	internal interface IMissionUiController : IUpdater {
		event Action<WeaponType> OnSkillChoose;
		
		void Init(IUnit player);
		void ShowSkillsChoose(ActualSkillInfo[] skills);
	}

}