using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace IdleCore.Attributes
{
    public abstract class UnitAttribute
    {
        public Image Icon;
        
        [HideInInspector] public int Value;
        [HideInInspector] public bool IsBuffingNow;
        
        protected Unit _unit;
        
        private QuestTracker _questTracker;
        
        public virtual void Init(Unit unit)
        {
            _unit = unit;
            
            Value = 0;
            Icon.gameObject.SetActive(false);
            OutBuffZone();
            
            _questTracker = new QuestTracker(QuestType());
            HouseManager.Instance.QuestManager.AddTracker(_questTracker);
        }

        public abstract UnitAttributes Type();
        protected abstract QuestType QuestType();

        public void SetIconStatus()
        {
            if (Value < 89)
            {
                if (Value > 75)
                    Icon.color = _unit.Config.LowNeed;
                else if (Value > 40)
                    Icon.color = _unit.Config.MidleNeed;
                else
                    Icon.color = _unit.Config.HighNeed;

                Icon.gameObject.SetActive(true);
            }
            else
            {
                Icon.gameObject.SetActive(false);
            }
        }

        public void HideIcon() => Icon.gameObject.SetActive(false);

        public virtual void ValueRandomDecrease()
        {
            if (IsBuffingNow == true)
                return;
            
            Value -= Random.Range(1, _unit.MaxDecreaseRate);
                    
            if (Value < 0)
                Value = 0;
        }

        public abstract void InBuffZone();

        public virtual void OutBuffZone()
        {
            IsBuffingNow = false;
        }

        public virtual void TakeBuff()
        {
            Value = _unit.MaxStatValue;
            Icon.gameObject.SetActive(false);
            
            _questTracker.ResponseQuest();
        }
    }
}