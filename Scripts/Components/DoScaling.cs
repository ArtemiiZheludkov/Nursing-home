using DG.Tweening;
using UnityEngine;

namespace IdleCore
{
    public class DoScaling : MonoBehaviour
    {
        [SerializeField] private Vector3 _targetScale;
        [SerializeField] private float _duration;

        private void Start()
        {
            transform.DOPunchScale(_targetScale, _duration, 1).SetLoops(-1).SetLink(gameObject);
        }
    }
}