using UnityEngine;

namespace FernandaDev
{
    public class UnitSelectionIndicator : MonoBehaviour
    {
        Renderer rend;

        [SerializeField] iTween.EaseType _animationEaseType;

        private void Awake()
        {
            rend = GetComponentInChildren<Renderer>();
            rend.enabled = false;

            Unit.OnUnitSelected += OnUnitSelected;
            Unit.OnUnitDeselected += OnUnitDeselected;
        }

        Transform targetUnit;

        void OnUnitSelected(Unit selectedUnit)
        {
            targetUnit = selectedUnit.transform;
            iTween.ScaleFrom(transform.GetChild(0).gameObject, iTween.Hash("scale", transform.localScale / 2, "time", .2f, "easetype", _animationEaseType));
            rend.enabled = true;
        }
        void OnUnitDeselected(Unit selectedUnit)
        {
            targetUnit = null;
            rend.enabled = false;
        }

        private void LateUpdate()
        {
            if (targetUnit != null)
                transform.position = targetUnit.position;
        }

        private void OnDestroy()
        {
            Unit.OnUnitSelected -= OnUnitSelected;
            Unit.OnUnitDeselected -= OnUnitDeselected;
        }
    }
}