using UnityEngine;

namespace IdleCore
{
    public interface IStaff
    {
        public StaffStatus Status { get; set; }
        public void SetStatus(StaffStatus status);

        public bool CanHasUnit();
        public bool TryTakeUnit(Unit unit);
        public Unit GetUnit();
        
        public bool TryTakeFood(GameObject food, float spaceStack);
        public GameObject TryGetFood();
        public void DropAllFood(Transform to);
    }
}