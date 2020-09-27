using UnityEngine;

public class UnitSelectionIndicator : MonoBehaviour
{
    Renderer rend;

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
