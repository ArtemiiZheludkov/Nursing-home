using UnityEngine;
using IdleCore.Attributes;

namespace IdleCore.Room
{
    public abstract class BuffRoom : Room
    {
        [Header("ZONE COMPONENTS")]
        [SerializeField] private Transform _unitOutZone;
        [SerializeField] private GameObject _effectOnUnit;
        [SerializeField] protected CircleFiller _progressBar;
        
        [Header("ZONE SETTINGS")]
        public UnitAttributes _buffStat;

        public override void Init()
        {
            base.Init();
            
            if (_effectOnUnit != null)
                _effectOnUnit.SetActive(false);
        }

        protected override void OnEnterUnit(Unit unit)
        {
            _progressBar.Fill(_config.WorkTime);
            UnitInRoom.InBuffZone(_buffStat);

            if (_effectOnUnit != null)
                _effectOnUnit.SetActive(true);
        }

        protected override void OnExitUnit()
        {
            base.OnExitUnit();
            
            _progressBar.Clear();
            
            if (_effectOnUnit != null)
                _effectOnUnit.SetActive(false);
        }

        protected override void AfterWork()
        {
            _progressBar.Clear();
            UnitInRoom.SetPosition(_unitOutZone);
            UnitInRoom.TakeBuff(_buffStat);

            if (_effectOnUnit != null)
                _effectOnUnit.SetActive(false);
        }
    }
}