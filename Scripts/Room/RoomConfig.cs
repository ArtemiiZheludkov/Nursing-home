using UnityEngine;

namespace IdleCore.Room
{
    [CreateAssetMenu(fileName = "newRoom", menuName = "Configs/Room")]
    public class RoomConfig : ScriptableObject
    {
        public float WorkTime;
        public float WaitForActivate;
        public float ReloadTime;
    }
}