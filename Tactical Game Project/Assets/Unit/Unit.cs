using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitData unitData;
    public Tile CurrentTile { get; private set; }

    public static event Action<Unit> OnUnitSelected;
    public static event Action<Unit> OnUnitDeselected;

    UnitUIDisplay unitUIDisplay;
    public MovementController movementController { get; private set; }

    private void Awake()
    {
        unitUIDisplay = GetComponentInChildren<UnitUIDisplay>();
        movementController = GetComponent<MovementController>();
    }

    private void Start()
    {
        UpdateCurrentTile();
        if(unitData) unitUIDisplay.SetupDisplay(unitData);
    }
    
    public void UpdateCurrentTile()
    {
        if(CurrentTile)
            CurrentTile.UnitOverTile = null; // before changing the new tile, we should reset the old one.

        CurrentTile = Map.Instance.GetCurrentTileFromWorld(transform.position);
        CurrentTile.UnitOverTile = this;
    }

    public void OnSelected()
    {
        OnUnitSelected?.Invoke(this);
        unitUIDisplay.commandMenuDisplay.ShowMenu(true);
    }

    public void OnDeselected()
    {
        OnUnitDeselected?.Invoke(this);
        unitUIDisplay.commandMenuDisplay.ShowMenu(false);
    }
}
