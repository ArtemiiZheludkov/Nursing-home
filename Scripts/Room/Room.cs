using System.Collections;
using IdleCore.Zone;
using UnityEngine;

namespace IdleCore.Room
{
    public abstract class Room : MonoBehaviour
    {
        [Header("ZONE COMPONENTS")]
        [SerializeField] protected EnterZone _enter;
        [SerializeField] protected ExitZone _exit;
        [SerializeField] protected Transform _unitInZone;

        [Header("ZONE SETTINGS")]
        [SerializeField] protected RoomConfig _config;

        [Header("STAFF INFO")]
        public Transform EnterStaff;
        [SerializeField] private TrashZone _trashZone;

        public Unit UnitInRoom { get; protected set; }
        [HideInInspector] public bool IsActive = false;
        
        public bool CanUse()
        {
            if (IsActive == true)
                if (_trashZone == null || _trashZone.PollutionLevel <= 75)
                    return true;

            return false;
        }

        public virtual void Init()
        {
            IsActive = true;
            
            UnitInRoom = null;
            _enter.Init(this, _config.WaitForActivate);
            _enter.Activate();
            _exit.Init(this, _config.WaitForActivate);
            _exit.Deactivate();
        }

        public virtual bool TryEnterUnit(Unit unit)
        {
            if (UnitInRoom != null)
            {
                _enter.Deactivate();
                return false;
            }
            
            if (unit == null)
            {
                StartCoroutine(ReloadZone());
                return true;
            }

            UnitInRoom = unit;
            UnitInRoom.SetPosition(_unitInZone);
            OnEnterUnit(UnitInRoom);
            
            StartCoroutine(StartZoneWork());
            return true;
        }

        public virtual void TryExitUnit(IStaff staff)
        {
            if (staff.TryTakeUnit(UnitInRoom) == true)
            {
                OnExitUnit();
                StartCoroutine(ReloadZone());
            }
        }
        
        protected abstract void OnEnterUnit(Unit unit);

        protected virtual void OnExitUnit()
        {
            UnitInRoom = null;
            StopAllCoroutines();
        }

        protected abstract void AfterWork();

        protected void StartZoneWorkCoroutine() => StartCoroutine(StartZoneWork());
        
        protected void ReloadZoneCoroutine() => StartCoroutine(ReloadZone());

        private IEnumerator StartZoneWork()
        {
            _enter.Deactivate();
            _exit.Deactivate();
            
            yield return new WaitForSeconds(_config.WorkTime);
            
            _enter.Deactivate();
            _exit.Activate();
            AfterWork();
        }

        private IEnumerator ReloadZone()
        {
            _enter.Deactivate();
            _exit.Deactivate();
            
            yield return new WaitForSeconds(_config.ReloadTime);
            
            UnitInRoom = null;
            _enter.Activate();
            _exit.Deactivate();
        } 
    }
}