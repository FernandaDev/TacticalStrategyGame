using System;
using System.Collections.Generic;
using UnityEngine;

public class CommandInputReader : MonoBehaviour
{
    #region Singleton
    public static CommandInputReader Instance { get; private set; }
    private void Awake() 
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(this);
    }
    #endregion

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
        currentUnit = MouseHandler.SelectedUnit;
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
                ShowSelectableTiles(MaterialType.Move);

                // 3. Tell the 'movementController' to create a path whenever the hovered tile has changed.
                // and don't forget to change the materials too.
                hoverCallbackListener = (t) => { currentUnit.movementController.CreatePath(t, SelectableTiles); };
                MouseHandler.Instance.OnHoverTileChangedCallback += hoverCallbackListener;

                //4. Set as the current command controller, so we can keep track of the command
                // and when the animation has ended, and then we'll be able to reset the selectable tiles.
                CurrentCommandController = currentUnit.movementController;

                break;
        }

        MouseHandler.SelectedUnit.OnTargetTileSelected += OnTileSelected;
    }

    void OnTileSelected(Tile selectedTile)
    {
        // If we clicked in a tile that is not selectable or there is a unit over it, we just bail out.
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

        // We don't need to listen to the hovering of the mouse anymore.
        MouseHandler.Instance.OnHoverTileChangedCallback -= hoverCallbackListener;
    }

    public void EndInputRead()
    {
        ResetSearch();
        OnReadCommandEnd?.Invoke();
    }

    void ResetSearch()
    {
        BFS.ResetSearchData(SelectableTiles);
        MouseHandler.SelectedUnit.OnTargetTileSelected -= OnTileSelected;
    }

    void ShowSelectableTiles(MaterialType materialType)
    {
        foreach (var tile in selectableTiles)
        {
            if (tile.HasUnitOver()) continue;

            tile.TileMaterialController.SetMaterialByType(materialType);
        }
    }
}
