using UnityEngine;

namespace IdleCore
{
    [CreateAssetMenu(fileName = "newStaff", menuName = "Configs/Staff")]
    public class StaffConfig : ScriptableObject
    {
        public string Name;
        public float WalkSpeed;
        public float RunSpeed;
        public float MinWorkSpeed;
        public float WorkSpeed;
        public int FoodCount;

        public int WorkerCount { get; private set; }
        
        public void LoadConfig()
        {
            WorkerCount = PlayerPrefs.GetInt(Name, 0);
            RunSpeed = PlayerPrefs.GetFloat(Name + "RunSpeed", RunSpeed);
            WorkSpeed = PlayerPrefs.GetFloat(Name + "WorkSpeed", WorkSpeed);
            FoodCount = PlayerPrefs.GetInt(Name + "FoodCount", FoodCount);
        }

        public void AddWorker()
        {
            WorkerCount += 1;
            PlayerPrefs.SetInt(Name, WorkerCount);
        }

        public void UpRunSpeed(float up)
        {
            RunSpeed += up;
            PlayerPrefs.SetFloat(Name + "RunSpeed", RunSpeed);
        }

        public void UpWorkSpeed(float up)
        {
            WorkSpeed -= up;

            if (WorkSpeed < MinWorkSpeed)
                WorkSpeed = MinWorkSpeed;
            
            PlayerPrefs.SetFloat(Name + "WorkSpeed", WorkSpeed);
        }

        public void UpFoodCount(int up)
        {
            FoodCount += up;
            PlayerPrefs.SetInt(Name + "FoodCount", FoodCount);
        }
    }
}