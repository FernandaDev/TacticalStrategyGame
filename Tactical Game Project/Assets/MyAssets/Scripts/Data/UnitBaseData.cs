using UnityEngine;

namespace FernandaDev
{
    [CreateAssetMenu(fileName = "Unit Base Data", menuName = "ScriptableObjects/Unit Base Data")]
    public class UnitBaseData : ScriptableObject
    {
        public string className;
        public string classDescription;

        public BaseStats classBaseStats;
    }
}