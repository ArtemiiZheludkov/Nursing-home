using UnityEngine;

namespace IdleCore
{
    [RequireComponent(typeof(BoxCollider))]
    public abstract class InteractionDelayZone : MonoBehaviour
    {
        [SerializeField] private StaffStatus _activateOnStaffStatus;
        protected float _waitForActivate;
        private float _activateTime;
        private bool _staffInZone;

        protected IStaff _currentStaff;

        private float _defaultWaitTime;

        public void Init(float waitForActivate)
        {
            _waitForActivate = waitForActivate;
            _defaultWaitTime = _waitForActivate;
            _activateTime = 0f;
            
            Activate();
        }

        public virtual void Activate()
        {
            gameObject.SetActive(true);
            _currentStaff = null;
        }

        public virtual void Deactivate()
        {
            gameObject.SetActive(false);
            _currentStaff = null;
        }

        public void SetWorkTime(float time)
        {
            _waitForActivate = time;
            _activateTime = Time.time + time;
        }

        public void SetDefaultTime()
        {
            _waitForActivate = _defaultWaitTime;
            _activateTime = 0f;
        }

        protected abstract void InteractWithStaff(IStaff staff);

        protected virtual void OnEnterStaff()
        {
        }
        
        protected virtual void OnExitStaff()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_currentStaff != null)
                return;
            
            if (other.TryGetComponent(out IStaff staff))
            {
                if (staff.Status == _activateOnStaffStatus)
                {
                    _activateTime = Time.time + _waitForActivate;
                    _currentStaff = staff;
                    OnEnterStaff();
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (Time.time < _activateTime) 
                return;
            
            if (_currentStaff != null)
            {
                if (_currentStaff.Status == _activateOnStaffStatus)
                {
                    InteractWithStaff(_currentStaff);
                    _activateTime = Time.time + _waitForActivate;
                }
            }
            else
            {
                if (other.TryGetComponent(out IStaff staff))
                {
                    if (staff.Status == _activateOnStaffStatus)
                    {
                        _activateTime = Time.time + _waitForActivate;
                        _currentStaff = staff;
                        OnEnterStaff();
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IStaff staff))
            {
                if (staff == _currentStaff) //staff.Status == _activateOnStaffStatus)
                {
                    _activateTime = 0f;
                    _currentStaff = null;
                    OnExitStaff();
                    SetDefaultTime();
                }
            }
        }
    }
}