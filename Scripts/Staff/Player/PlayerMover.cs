using UnityEngine;

namespace IdleCore
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotateSpeed;

        private Rigidbody _rigidbody;
        private Animator _animator;
        private HashAnimation _animations;
        
        private bool _isStay;
        private float _pastDistance;
        private Vector3 _offset;

        public void Init(Animator animator, HashAnimation animations)
        {
            _animator = animator;
            _animations = animations;
            
            _rigidbody = GetComponent<Rigidbody>();
            _joystick.gameObject.SetActive(true);
            _isStay = true;
            _animator.CrossFade(_animations.Idle, 0.1f);
        }

        private void Update()
        {
            if (_joystick.Horizontal != 0f || _joystick.Vertical != 0f)
            {
                Move();
                Rotate();
            }
            else if (_isStay == false)
            {
                _animator.CrossFade(_animations.Idle, 0.1f);
                _rigidbody.velocity = Vector3.zero;
                _isStay = true;
            }
        }
        
        private void Move()
        {
            if (_isStay == true)
            {
                _animator.CrossFade(_animations.Run, 0.1f);
                _isStay = false;
            }
            
            _offset = new Vector3(_joystick.Horizontal, 0.0f, _joystick.Vertical) * _moveSpeed * Time.deltaTime;
            _rigidbody.MovePosition(_rigidbody.position + _offset);
        }
        
        private void Rotate()
        {
            Vector3 rotationLookAtVector = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);

            if (rotationLookAtVector == Vector3.zero)
                return;

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(rotationLookAtVector),
                _rotateSpeed * Time.deltaTime);
        }
    }
}