using System;
using System.Collections.Generic;
using UnityEngine;

public class CommandInputReader : MonoBehaviour
{
    private List<Tile> selectableTiles = new List<Tile>();
    public List<Tile> SelectableTiles => selectableTiles;

    Unit unit;

    CommandType _commandType = CommandType.Default;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public void ReadInputCommand(CommandType commandType)
    {
        _commandType = commandType;

        switch (commandType)
        {
            case CommandType.Move:
                selectableTiles = BFS.SearchTiles(unit.CurrentTile, unit.unitData.MovementDistance, true);

                foreach (var tile in selectableTiles)
                {
                    tile.TileMaterialController.SetMaterialByType(commandType);
                }
                break;
        }

        SelectionHandler.Instance.SelectionState = SelectionState.CommandTileSelection;
        SelectionHandler.OnTileSelected += OnTileSelected;
    }

    public void OnTileSelected(Tile selectedTile)
    {
        if (!selectableTiles.Contains(selectedTile)) return;

        switch (_commandType)
        {
            case CommandType.Move:
                //check for manapoints
                //create the new command
                ICommand _command = new MoveCommand(unit.GetComponent<MovementController>(),
                                                    unit.CurrentTile,
                                                    selectedTile);
                //add to the invoker's list
                CommandInvoker.AddCommand(_command);

                unit.GetComponent<MovementController>().OnMovementEnd = ResetCommandInput;

                break;
        }
       
    }

    void ResetCommandInput()
    {
        _commandType = CommandType.Default;
        BFS.ResetSearchData(selectableTiles);
        SelectionHandler.Instance.DeselectUnit();
        SelectionHandler.Instance.SelectionState = SelectionState.UnitSelection;
        SelectionHandler.OnTileSelected -= OnTileSelected;
    }
}
