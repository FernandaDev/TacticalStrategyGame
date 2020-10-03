using System;
using UnityEngine;

namespace FernandaDev
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] float currentHealth;
        public float CurrentHealth
        {
            get { return currentHealth; }
            private set
            {
                if (value != currentHealth)
                {
                    currentHealth = value;
                    currentHealth = Mathf.Clamp(currentHealth, 0, maxHP);
                    OnHealthChangeCallback?.Invoke(currentHealth);
                }
            }
        }

        public event Action<float> OnHealthChangeCallback;
        float maxHP;

        private void Awake()
        {
            maxHP = GetComponent<Unit>().unitBaseData.classBaseStats.MaxHP;
        }

        private void Start()
        {
            CurrentHealth = maxHP;
        }

        public void TakeDamage(float damageAmount)
        {
            CurrentHealth += damageAmount;

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            Debug.Log($"{name} has died!");
        }

        private void OnDestroy()
        {
            OnHealthChangeCallback = null;
        }

    }
}