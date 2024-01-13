using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace IdleCore
{
    public class CircleFiller : MonoBehaviour
    {
        [SerializeField] private Image _progressBar;

        private Tween _tween;
        
        private void Start()
        {
            _progressBar.fillAmount = 0f;
        }

        public void Fill(in float time, bool isLooping = false)
        {
            _progressBar.fillAmount = 0f;
            _tween = _progressBar.DOFillAmount(1, time)
                .SetEase(Ease.Linear)
                .SetLink(gameObject)
                .OnComplete(() => _progressBar.fillAmount = 0f);
            
            if (isLooping == true)
                _tween.SetLoops(-1);
        }
        
        public void Clear()
        {
            _progressBar.fillAmount = 0f;
            _tween.Kill();
        }
    }
}