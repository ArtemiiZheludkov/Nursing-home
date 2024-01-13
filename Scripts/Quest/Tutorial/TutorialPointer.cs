using UnityEngine;

namespace IdleCore
{
    public class TutorialPointer : MonoBehaviour
    {
        [SerializeField] private RectTransform _myRect;
        [SerializeField] private RectTransform _canvasRectTransform;
        private Transform _target;
        private Camera _camera;

        private void Update() 
        {
            if (_target == null)
                gameObject.SetActive(false);
            
            Vector3 target = _camera.WorldToViewportPoint(_target.position);

            Vector3 onScreen = new Vector3(
                Mathf.Clamp(target.x, 0.05f, 0.95f) * Screen.width,
                Mathf.Clamp(target.y, 0.025f, 0.975f) * Screen.height,
                0f);
                
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, onScreen, null, out Vector2 localPosition);
            _myRect.localPosition = localPosition;
        }

        public void Init()
        {
            _camera = Camera.main;
            gameObject.SetActive(true);
        }

        public void PointOnTarget(Transform target)
        {
            _target = target;
            gameObject.SetActive(true);
        }
        
        public void Deactivate()
        {
            _target = null;
            gameObject.SetActive(false);
        }
    }
}