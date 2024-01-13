using UnityEngine;

namespace IdleCore
{
    [CreateAssetMenu(fileName = "newUnit", menuName = "Configs/UnitConfig")]
    public class UnitConfig : ScriptableObject
    {
        public float DecreaseRate;
        public int MaxDecreaseInRate;
        public int SalaryForService;

        [Header("Background color for UI need")]
        public Color LowNeed;
        public Color MidleNeed;
        public Color HighNeed;
    }
}