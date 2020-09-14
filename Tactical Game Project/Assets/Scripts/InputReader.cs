using System;
using System.Collections.Generic;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    public static InputReader Instance { get; private set; }

    private void Awake() 
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(this);
    }

    Unit currentUnit;
    CommandType currentCommandType;
    public ICommandController CurrentCommandController { get; private set; }

    public static event Action OnReadCommandStart;
    public static event Action OnReadCommandEnd;
    Action<Tile> hoverCallbackListener;

    private List<Tile> selectableTiles  = new List<Tile>();
    public List<Tile> SelectableTiles { get => selectableTiles; private set => selectableTiles = value; }

    public void ReadInput(CommandType commandType)
    {
        currentUnit = MouseHandler.Instance.SelectedUnit;
        currentCommandType = commandType;

        StartReading();
    }

    void StartReading()
    {
        OnReadCommandStart?.Invoke();

        switch (currentCommandType)
        {
            case CommandType.Move:
                // 1. Search for the selectable tiles.
                SelectableTiles = BFS.SearchTiles(currentUnit.CurrentTile, currentUnit.unitData.MovementDistance);

                // 2. Change the tiles material
                ShowSelectableTiles(CommandType.Move);

                // 3. Tell the 'movementController' to create a path whenever the hovered tile has changed.
                // and don't forget to change the materials too.
                hoverCallbackListener = (t) => { currentUnit.movementController.CreatePath(t, SelectableTiles); };
                MouseHandler.Instance.OnHoverTileChangedCallback += hoverCallbackListener;

                //4. Set as the current command controller, so we can keep track of the command
                // and when the animation has ended, and then we'll be able to reset the selectable tiles.
                CurrentCommandController = currentUnit.movementController;

                break;
        }

        MouseHandler.Instance.OnTileSelected += OnTileSelected;
    }

    void OnTileSelected(Tile selectedTile)
    {
        // If we clicked in a tile that is not selectable or there is a unit over it, we just bail out.
        //if (!selectableTiles.Contains(selectedTile) || selectedTile.HasUnitOver()) return; //TODO this only works for movement.
        if (!SelectableTiles.Contains(selectedTile) || 
            !CurrentCommandController.IsSelectionViable(selectedTile)) return;

        // Here we are going to create the command and send to the CommandInvoker to excute it.
        switch (currentCommandType)
        {
            case CommandType.Move:
                ICommand newCommand = new MoveCommand(currentUnit.movementController, currentUnit.CurrentTile, selectedTile);
                CommandInvoker.AddCommand(newCommand);
                break;
        }

        // We need to know when the command animation is fishined, so then we can reset the selectable tiles.
        CurrentCommandController.OnCommandAnimationEnd += EndReading;

        // We don't need to listen to the hovering of the mouse anymore.
        MouseHandler.Instance.OnHoverTileChangedCallback -= hoverCallbackListener;

        //MouseHandler.Instance.CanSelect = false;
    }

    void EndReading()
    {
        currentUnit = null;
        CurrentCommandController.OnCommandAnimationEnd -= EndReading;
        BFS.ResetSearchData(SelectableTiles);
        MouseHandler.Instance.OnTileSelected -= OnTileSelected;
        OnReadCommandEnd?.Invoke();
    }

    void ShowSelectableTiles(CommandType commandType)
    {
        foreach (var tile in selectableTiles)
        {
            if (tile.HasUnitOver()) continue;

            tile.TileMaterialController.SetMaterialByType(commandType);
        }
    }
}
