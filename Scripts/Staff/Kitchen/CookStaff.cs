using IdleCore.Room;
using UnityEngine;
using UnityEngine.AI;

namespace IdleCore
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Cook))]
    public class CookStaff : MonoBehaviour, IStaff
    {
        public StaffStatus Status { get; set; }

        [SerializeField] private StaffConfig _config;
        [SerializeField] private Animator _animator;
        
        private Cooker _cooker;
        private KitchenTable _kitchenTable;
        private DinnerTable _dinnerTable;
        private FoodCollector _foodCollector;

        private HashAnimation _animations;
        private NavMeshAgent _agent;
        private Cook _cook;

        private Vector3 _target;
        private bool _hasTarget;

        public void Init(Cooker cooker, KitchenTable kitchenTable, DinnerTable dinnerTable, FoodCollector foodCollector)
        {
            _cooker = cooker;
            _kitchenTable = kitchenTable;
            _dinnerTable = dinnerTable;
            _foodCollector = foodCollector;
            
            _agent = GetComponent<NavMeshAgent>();
            _animations = new HashAnimation();

            _cook = GetComponent<Cook>();
            _cook.Init(this, _config.FoodCount);

            SetStatus(StaffStatus.Free);
        }

        private void FixedUpdate()
        {
            if (_hasTarget == true && _agent.remainingDistance < _agent.stoppingDistance)
                SetJob();
        }
        
        public float GetWorkTime() => _config.WorkSpeed;

        private void MoveTo(Transform to)
        {
            if (Vector3.Distance(transform.position, to.position) < 1f)
            {
                _hasTarget = false;
                _animator.Play(_animations.Idle, 0);
                return;
            }
            
            _hasTarget = true;
            
            _target = to.position;
            _target.y = 0f;
            _agent.speed = _config.RunSpeed;
            _agent.SetDestination(_target);
            _animator.CrossFade(_animations.Run, 0.1f);
        }

        public void SetJob()
        {
            _hasTarget = false;
            
            if (_dinnerTable.IsFull() == false)
            {
                if (Status == StaffStatus.HasFood)
                    MoveTo(_dinnerTable.WorkerPlace);
                else if (_kitchenTable.Foods > 0)
                    MoveTo(_kitchenTable.transform);
                else if (_cooker.CanWork(this) == true)
                    MoveTo(_cooker.transform);
                else
                    RandomWalk();
            }
            else
            {
                if (Status == StaffStatus.HasFood)
                    MoveTo(_foodCollector.transform);
                else
                    RandomWalk();
            }
        }

        public void SetStatus(StaffStatus status)
        {
            Status = status;

            if (Status == StaffStatus.Free)
                _animator.Play(_animations.HandFree, 1);

            if (Status == StaffStatus.HasFood)
                _animator.Play(_animations.HoldStack, 1);
            
            SetJob();
        }
        
        private void RandomWalk()
        {
            _hasTarget = true;
            
            Vector3 randomPos = Random.insideUnitSphere * 10f;
            randomPos.y = 0f;
            _agent.SetDestination(randomPos);
            _agent.speed = _config.WalkSpeed;
            _animator.Play(_animations.Walk, 0);
        }

        #region Staff

        public bool CanHasUnit() => false;

        public bool TryTakeUnit(Unit unit) => false;

        public Unit GetUnit() => null;

        public bool TryTakeFood(GameObject food, float spaceStack) =>  _cook.TryTakeFood(food, in spaceStack);

        public GameObject TryGetFood() => _cook.TryGetFood();

        public void DropAllFood(Transform to) => _cook.DropAllFood(to);

        #endregion
    }
}