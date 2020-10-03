using UnityEngine;
using TMPro;

namespace FernandaDev
{
    public class UnitUIDisplay : MonoBehaviour
    {
        public TextMeshProUGUI unitName;
        public BarDisplay hpBar;
        public BarDisplay mpBar;
        public CommandMenuDisplay commandMenuDisplay; //TODO do we need this?

        UnitBaseData unitBaseData;

        private void Awake()
        {
            unitBaseData = GetComponentInParent<Unit>().unitBaseData;
            GetComponentInParent<Damageable>().OnHealthChangeCallback += OnHealthChanged;
        }

        private void Start()
        {
            if (unitName) unitName.text = unitBaseData.className;

            hpBar.SetupValues(unitBaseData.classBaseStats.MaxHP);
            mpBar.SetupValues(unitBaseData.classBaseStats.MaxMP);
        }

        void OnHealthChanged(float newHP) => hpBar.RefreshValues(newHP);

        void OnManaChaged(float newMP) { }
    }
}