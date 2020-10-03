using UnityEngine;

namespace FernandaDev
{
    [System.Serializable]
    public struct BaseStats
    {
        [SerializeField] float maxHP;
        public float MaxHP => maxHP;

        [SerializeField] float maxMP;
        public float MaxMP => maxMP;

        [SerializeField] float attackPoints;
        public float AttackPoints => attackPoints;

        [SerializeField] float defensePoints;
        public float DefensePoints => defensePoints;

        [SerializeField] int movementPoints;
        public int MovementPoints => movementPoints;

        [SerializeField] int attackRange;
        public int AttackRange => attackRange;
    }
}