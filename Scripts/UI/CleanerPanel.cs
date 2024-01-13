using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleCore
{
    public class CleanerPanel : StaffPanel
    {
        [Header("WORK SPEED")]
        [SerializeField] private int _workPrice;
        [SerializeField] private float _upWorkSpeed;
        [SerializeField] private Button _workButton;
        [SerializeField] private TMP_Text _workText;
        
        [Space(5)]
        [SerializeField] private ClearStaff _prefab;

        public override void Init(Transform start, HouseBank bank)
        {
            base.Init(start, bank);
            
            _workPrice = PlayerPrefs.GetInt(_config.name + "WorkPrice", _workPrice);
            _workText.text = _workPrice.ToString();
            
            _workButton.onClick.AddListener(OnWorkClicked);
        }

        private void OnWorkClicked()
        {
            if (Bank.TryGetCoins(_workPrice) == false)
                return;
            
            _workPrice += (int)(_workPrice * UpPrice);
            _workText.text = _workPrice.ToString();
            PlayerPrefs.SetInt(_config.name + "WorkPrice", _workPrice);
            
            _config.UpWorkSpeed(_upWorkSpeed);
        }
        
        protected override void CreateWorker()
        {
            Instantiate(_prefab, StartWorker.position, Quaternion.identity);
        }
    }
}