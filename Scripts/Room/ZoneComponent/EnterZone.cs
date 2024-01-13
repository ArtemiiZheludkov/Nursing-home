using UnityEngine;

namespace IdleCore.Room
{
    public class EnterZone : InteractionDelayZone
    {
        private Room _zone;

        public void Init(Room zone, float waitForActivate)
        {
            _zone = zone;
            base.Init(waitForActivate);
        }

        protected override void InteractWithStaff(IStaff staff)
        {
            Unit unit = staff.GetUnit();

            if (_zone.TryEnterUnit(unit) == false)
                staff.TryTakeUnit(unit);
        }
    }
}