using System;
using Infrastructure;
using Services;
using Units;
using Weapons;


namespace UI
{
    internal sealed class SkillsUiController : IDisposable
    {
        public event Action<WeaponType> OnSkillChoose;

        private readonly UiConfig _uiConfig;
        private readonly SkillsPanelView _skillsPanel;
        private readonly IViewsFactory _viewsFactory;


        public SkillsUiController(SkillsPanelView skillsPanel, IViewsFactory viewsFactory, UiConfig uiConfig)
        {
            _skillsPanel = skillsPanel;
            _viewsFactory = viewsFactory;
            _uiConfig = uiConfig;
        }

        public void Dispose()
        {
            _skillsPanel.OnSkillChoosen -= UpgradeSkill;
            _skillsPanel.OnDispose();
        }

        public void Init()
        {
            _skillsPanel.Init(_viewsFactory, _uiConfig);
            _skillsPanel.OnSkillChoosen += UpgradeSkill;
        }

        public void ShowSkillsChoose(ActualSkillInfo[] skills)
        {
            _skillsPanel.ShowSkills(skills);
        }

        private void UpgradeSkill(ActualSkillInfo skill)
        {
            OnSkillChoose?.Invoke(skill.WeaponType);
        }
    }
}