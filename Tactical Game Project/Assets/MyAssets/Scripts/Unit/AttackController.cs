using System;
using System.Collections;
using UnityEngine;

namespace FernandaDev
{
    public class AttackController : MonoBehaviour, ICommandController
    {
        public event Action OnCommandAnimationEnd;
        public Unit myUnit { get; private set; }
        public CommandType commandType { get; private set; }
        public float BaseDamage => myUnit.unitBaseData.classBaseStats.AttackPoints;

        private void Awake() 
        { 
            myUnit = GetComponent<Unit>();
            commandType = CommandType.Attack;
        }

        public void StartAttack(Tile targetTile)
        {
            StartCoroutine(Attack(targetTile));
        }

        IEnumerator Attack(Tile targetTile)
        {
            yield return new WaitForSeconds(1f);

            if (targetTile.HasUnitOver())
            {
                var enemyDamageable = targetTile.UnitOverTile.GetComponent<Damageable>();

                if (enemyDamageable)
                {
                    enemyDamageable.TakeDamage(-BaseDamage);
                }
                else Debug.LogError("The unit attacked is not Damageable.");
            }
            EndAttack();

            yield return null;
        }

        void EndAttack()
        {
            OnCommandAnimationEnd?.Invoke();
        }

        public void CancelAttack(Unit attackedUnit)
        {
            var enemyDamageable = attackedUnit.GetComponent<Damageable>();
            if (enemyDamageable)
                enemyDamageable.TakeDamage(BaseDamage);
        }

        public bool IsSelectionViable(Tile selectedTile) // for now we decided that we can attack anyone, even teammates. =)
        {
            if (selectedTile.HasUnitOver() && selectedTile.UnitOverTile == myUnit) return false;

            return true;
        }
    }
}