using System;
using UnityEngine;

namespace IdleCore
{
    [RequireComponent(typeof(PlayerMover))]
    [RequireComponent(typeof(Cook))]
    [RequireComponent(typeof(Caretaker))]
    public class Player : MonoBehaviour, IStaff
    {
        public event Action<StaffStatus> UpdatedPlayerStatus;
        public StaffStatus Status { get; set; }

        [SerializeField] private Animator _animator;
        
        private PlayerMover _mover;
        private Cook _cook;
        private Caretaker _helper;
        private HashAnimation _animations;

        public void Init()
        {
            _animations = new HashAnimation();
            _mover = GetComponent<PlayerMover>();
            _mover.Init(_animator, _animations);

            _cook = GetComponent<Cook>();
            _cook.Init(this);
            
            _helper = GetComponent<Caretaker>();
            _helper.Init(this);

            Status = StaffStatus.Free;
            UpdatedPlayerStatus?.Invoke(Status);
        }

        public void SetStatus(StaffStatus status)
        {
            Status = status;
            UpdatedPlayerStatus?.Invoke(Status);

            if (Status == StaffStatus.Free)
                _animator.Play(_animations.HandFree, 1);

            if (Status == StaffStatus.HasFood)
                _animator.Play(_animations.HoldStack, 1);

            if (Status == StaffStatus.HasUnit)
                _animator.Play(_animations.CarryUnit, 1);
        }

        #region Staff

        public bool CanHasUnit() => true;
        
        public bool TryTakeUnit(Unit unit) => _helper.TryTakeUnit(unit);

        public Unit GetUnit() => _helper.GetUnit();

        public bool TryTakeFood(GameObject food, float spaceStack) => _cook.TryTakeFood(food, in spaceStack);

        public GameObject TryGetFood() => _cook.TryGetFood();

        public void DropAllFood(Transform to) =>  _cook.DropAllFood(to);

        #endregion
    }
}
