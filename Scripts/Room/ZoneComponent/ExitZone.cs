using UnityEngine;

namespace IdleCore.Room
{
    public class ExitZone : InteractionDelayZone
    {
        [SerializeField] private CircleFiller _progressBar;
        private Room _zone;

        public void Init(Room zone, float waitForActivate)
        {
            _zone = zone;
            base.Init(waitForActivate);
        }
        
        protected override void InteractWithStaff(IStaff staff)
        {
            _zone.TryExitUnit(staff);
        }
        
        protected override void OnEnterStaff()
        {
            if(_currentStaff.CanHasUnit() == false)
                return;
            
            _progressBar.Fill(_waitForActivate);
        }
        
        protected override void OnExitStaff()
        {
            _progressBar.Clear();
        }
    }
}