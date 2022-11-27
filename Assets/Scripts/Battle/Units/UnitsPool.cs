using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using Units;
using UnityEngine;
using Utils;
using Zenject;


namespace Services
{
    internal sealed class UnitsPool : ObjectsPool<IUnit>, IUnitsPool
    {
        public List<IUnit> ActiveMonsters => SpawnedObjects;
        private readonly IUnitsFactory _unitsFactory;
        private readonly IHandleDamageManager _handleDamageManager;
        private readonly IMissionResultManager _missionResultManager;


        [Inject]
        public UnitsPool(IUnitsFactory unitsFactory, IHandleDamageManager handleDamageManager, IPhotonManager photonManager
            , IPunEventRaiser punEventRaiser, IMissionResultManager missionResultManager)
        {
            _unitsFactory = unitsFactory;
            _handleDamageManager = handleDamageManager;
            PunEventRaiser = punEventRaiser;
            _missionResultManager = missionResultManager;
            PhotonManager = photonManager;
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (var unit in Objects.Values.SelectMany(obj => obj))
            {
                unit.OnDied -= ReturnUnit;
                unit.OnDied -= StopUnitPeriodicalDamage;
                unit.OnDied -= _missionResultManager.CountKill;
                unit.Dispose();
            }

            foreach (var objList in Objects.Values)
            {
                objList.Clear();
            }

            Objects.Clear();
            foreach (var unit in ActiveMonsters)
            {
                unit.OnDied -= ReturnUnit;
                unit.OnDied -= StopUnitPeriodicalDamage;
                unit.OnDied -= _missionResultManager.CountKill;
                unit.Dispose();
            }

            ActiveMonsters.Clear();
        }

        protected override int GetSpecifiedPoolIndex(object[] parameters)
        {
            return (int)parameters[0];
        }

        protected override async Task<IUnit> SpawnSpecifiedObjectAsync(Vector2 position, Quaternion rotation, object[] parameters)
        {
            var currentMonsterLevel = (int)parameters[0];
            var unit = await _unitsFactory.CreateMyMonsterAsync(currentMonsterLevel, position, rotation);
            unit.OnDied += ReturnUnit;
            unit.OnDied += StopUnitPeriodicalDamage;
            unit.OnDied += _missionResultManager.CountKill;
            return unit;
        }

        private void ReturnUnit(DamageInfo damageInfo)
        {
            ReturnObject(damageInfo.DamageTaker);
        }

        private void StopUnitPeriodicalDamage(DamageInfo damageInfo)
        {
            _handleDamageManager.StopPeriodicalDamageForUnit(damageInfo.DamageTaker);
        }
    }
}