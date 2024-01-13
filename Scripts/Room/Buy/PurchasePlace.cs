using System;
using DG.Tweening;
using UnityEngine;

namespace IdleCore.Room
{
    public class PurchasePlace : MonoBehaviour
    {
        [SerializeField] private bool _activatedOnStart;
        
        [Header("BUY ZONE")]
        [SerializeField] private int _startPrice;
        [SerializeField] private BuyZone _buyZone;
        
        [Header("ROOM ZONE")]
        [SerializeField] private Room _room;
        public bool IsLifeRoom = false;
        [SerializeField] private ParticleSystem _buyEffect;
        [SerializeField] private QuestType _questType;

        private int Id;
        private bool _isActivated;
        
        private string RoomIdActivatedKey = "RoomActivated";
        private string RoomIdPriceKey = "RoomPrice";

        private Action<Unit> _roomActivated;
        private QuestTracker _quest;

        public void Init(Action<Unit> roomActivated = null)
        {
            _roomActivated = roomActivated;
            
            Id = gameObject.GetInstanceID();
            RoomIdActivatedKey += Id.ToString();

            int activated = _activatedOnStart == true ? 1 : 0;
            activated = PlayerPrefs.GetInt(RoomIdActivatedKey, activated);

            if (activated <= 0)
                InitBuyState();
            else
                InitRoom();
        }

        public void RoomBought()
        {
            PlayerPrefs.SetInt(RoomIdActivatedKey, 1);
            InitRoom();
            _room.transform.localScale = Vector3.zero;

            DOTween.Sequence()
                .Append(_room.transform.DOScale(Vector3.one, 0.45f))
                .Append(_room.transform.DOShakePosition(0.4f, Vector3.up, 1))
                .SetLink(_room.gameObject);
            
            _buyEffect.Play();
            
            _quest?.ResponseQuest();
            HouseManager.Instance.QuestManager.RemoveTracker(_quest);
            HouseManager.Instance.HRManager.CaretakerManager.FindRooms();
        }

        private void InitRoom()
        {
            _isActivated = true;
            _buyZone.gameObject.SetActive(false);
            _room.gameObject.SetActive(_isActivated);

            _room.Init();
            _roomActivated?.Invoke(_room.UnitInRoom);
        }

        private void InitBuyState()
        {
            _isActivated = false;
            _buyZone.gameObject.SetActive(true);
            _room.gameObject.SetActive(_isActivated);

            RoomIdPriceKey += Id.ToString();
            _buyZone.Init(_startPrice, RoomIdPriceKey);
            
            _quest = new QuestTracker(_questType);
            HouseManager.Instance.QuestManager.AddTracker(_quest);
        }
    }
}