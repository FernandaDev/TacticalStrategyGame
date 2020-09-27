using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitData unitData;
    public Tile CurrentTile { get; private set; }

    public static event Action<Unit> OnUnitSelected;
    public static event Action<Unit> OnUnitDeselected;
    
    public event Action<Tile> OnTargetTileSelected;

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

    public void OnGetSelected()
    {
        OnUnitSelected?.Invoke(this);
        unitUIDisplay.commandMenuDisplay.ShowMenu(true);
    }

    public void OnGetDeselected()
    {
        OnUnitDeselected?.Invoke(this);
        unitUIDisplay.commandMenuDisplay.ShowMenu(false);
    }

    public void OnCommandTileSelected(Tile selectedTile)
    {
        OnTargetTileSelected?.Invoke(selectedTile);
        unitUIDisplay.commandMenuDisplay.OnCommandTileSelected();
        CommandInvoker.OnCommandConfirmationRequest += unitUIDisplay.commandMenuDisplay.OnConfirmingCommand; // PS. the event will unsubscribe all the listener when the command is done.
    }
}