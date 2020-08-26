using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitData unitData;
    [SerializeField]Tile currentTile;
    public Tile CurrentTile { get => currentTile; private set => currentTile = value; }

    public static event Action<Unit> OnUnitSelected;
    public static event Action<Unit> OnUnitDeselected;

    public Menu unitMenu { get; private set; } //TODO decide if we realy need this reference here.

    private void Awake()
    {
        CurrentTile = Map.Instance.GetCurrentTileFromWorld(transform.position);
        unitMenu = GetComponentInChildren<Menu>();
    }

    private void Start() 
    {
        UpdateCurrentTile();
    }
    
    public void UpdateCurrentTile()
    {
        currentTile.UpdateUnitOverTile(null); // before changing the new tile, we should reset the old one.

        currentTile = Map.Instance.GetCurrentTileFromWorld(transform.position);
        currentTile.UpdateUnitOverTile(this);
    }

    public void OnSelected()
    {
        OnUnitSelected?.Invoke(this);
        unitMenu.ShowMenu(true);
    }

    public void OnDeselected()
    {
        OnUnitDeselected?.Invoke(this);
        unitMenu.ShowMenu(false);
    }
}
