using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace IdleCore
{
    public class Cook : MonoBehaviour
    {
        [SerializeField] private Transform _hands;
        [SerializeField] private int _maxStack = 1;
        
        private IStaff _staff;
        private List<GameObject> _foods = new List<GameObject>();

        public void Init(IStaff staff, int maxStack = 0)
        {
            _staff = staff;

            if (maxStack > 1)
                _maxStack = maxStack;
        }

        public bool TryTakeFood(GameObject food, in float spaceStack)
        {
            if (_staff.Status == StaffStatus.HasUnit)
                return false;

            if (_foods.Count >= _maxStack)
                return false;
            
            _foods.Add(food);
            food.transform.SetParent(_hands);

            Vector3 to = new Vector3(0f, (_foods.Count - 1) * spaceStack, 0f);
            
            DOTween.Sequence()
                .Append(food.transform.DOLocalJump(to, 2, 1, 0.4f))
                .Append(food.transform.DOPunchScale(Vector3.one * 0.5f, 0.5f, vibrato: 1))
                .SetLink(food);
            
            _staff.SetStatus(StaffStatus.HasFood);
            
            return true;
        }

        public GameObject TryGetFood()
        {
            if (_foods.Count < 1)
                return null;

            int last = _foods.Count - 1;
            GameObject food = _foods[last];
            OnGetInStack(last);
            
            return food;
        }

        public void DropAllFood(Transform to)
        {
            if (_foods.Count > 0)
            {
                for (int i = 0; i < _foods.Count; i++)
                {
                    GameObject food = _foods[i];
                    food.transform.DOJump(to.position, 2, 1, 0.4f)
                        .SetLink(_foods[i])
                        .OnComplete(() => Destroy(food));
                }
                
                _foods.Clear();
                _staff.SetStatus(StaffStatus.Free);
            }
            //StartCoroutine(DropAllStack(to));
        }

        private IEnumerator DropAllStack(Transform to)
        {
            for (int i = 0; i < _foods.Count; i++)
            {
                _foods[i].transform.DOJump(to.position, 2, 1, 0.4f)
                    .SetLink(_foods[i]);
                
                yield return new WaitForSeconds(0.1f);
            }
            
            yield return null;

            foreach (GameObject obj in _foods)
                Destroy(obj);
                        
            _foods.Clear();
            _staff.SetStatus(StaffStatus.Free);
        }

        private void OnGetInStack(int index)
        {
            if (_foods.Count <= 1)
            {
                _foods.Clear();
                _staff.SetStatus(StaffStatus.Free);
            }
            else
            {
                _foods.RemoveAt(index);
            }
        }
    }
}