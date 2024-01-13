using TMPro;
using UnityEngine;

namespace IdleCore.Room
{
    [RequireComponent(typeof(BoxCollider))]
    public class BuyZone : MonoBehaviour
    {
        [SerializeField] private PurchasePlace _room;
        [SerializeField] private TMP_Text _priceTxt;
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private Transform _lookParticle;
        [SerializeField] private float _buyTime;
        [SerializeField] private float _particleCooldown;
        
        private HouseBank Bank;
        
        private string RoomIdPriceKey;
        private int _price;

        private int _payMoney;
        private float _waitForPay;
        private float _nextPayTime;
        private float _particleTime;

        public void Init(int price, string roomIdPriceKey)
        {
            Bank = HouseManager.Instance.Bank;
            
            _price = price;
            RoomIdPriceKey = roomIdPriceKey;
            
            _price = PlayerPrefs.GetInt(RoomIdPriceKey, _price);

            _waitForPay = 0.1f;
            _payMoney = _price / (int)(_buyTime / _waitForPay);
            
            if (_payMoney < 1)
                _payMoney = 1;

            _nextPayTime = 0f;
            _particleTime = 0f;
            
            UpdatePrice();
        }

        private void UpdatePrice()
        {
            if (_price <= 0)
                _room.RoomBought();
            else
                _priceTxt.text = _price.ToString();
        }
        
        private void PlayMoneyParticles(Transform to)
        {
            if (Time.time < _particleTime)
                return;
            
            Vector3 particlePos = to.position;
            particlePos.y = 1f;

            _particle.transform.position = particlePos;
            _particle.transform.LookAt(_lookParticle);
            _particle.Play();
                
            _particleTime = Time.time + _particleCooldown;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                if (Bank.TryGetCoins(_payMoney) == false)
                    return;
                
                _price -= _payMoney;
                _nextPayTime = Time.time + _waitForPay;

                UpdatePrice();
                PlayMoneyParticles(player.transform);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (Time.time < _nextPayTime) 
                return;
            
            if (other.TryGetComponent(out Player player))
            {
                if (Bank.TryGetCoins(_payMoney) == false)
                    return;
                
                _price -= _payMoney;
                _nextPayTime = Time.time + _waitForPay;
                
                UpdatePrice();
                PlayMoneyParticles(player.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                PlayerPrefs.SetInt(RoomIdPriceKey, _price);
                UpdatePrice();
                _particle.Stop();
            }
        }
    }
}