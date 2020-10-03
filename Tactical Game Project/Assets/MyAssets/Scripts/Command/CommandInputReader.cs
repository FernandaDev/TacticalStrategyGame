using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FernandaDev
{
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

        PlayerUnit currentUnit;
        public CommandType currentCommandType { get; private set; }
        public ICommandController CurrentCommandController { get; private set; }

        public static event Action OnReadCommandStart;
        public static event Action OnReadCommandEnd;

        [SerializeField]private List<Tile> selectableTiles = new List<Tile>();
        public List<Tile> SelectableTiles { get => selectableTiles;}

        public void ReadInput(CommandType commandType)
        {
            currentUnit = SelectionController.SelectedUnit;
            currentCommandType = commandType;

            StartReading();
        }

        void StartReading()
        {
            OnReadCommandStart?.Invoke();

            switch (currentCommandType)
            {
                case CommandType.Move:
                    selectableTiles = BFS.SearchTiles(currentUnit.CurrentTile, currentUnit.unitBaseData.classBaseStats.MovementPoints);
                    CurrentCommandController = currentUnit.movementController;

                    break;
                case CommandType.Attack:
                    selectableTiles = BFS.SearchTiles(currentUnit.CurrentTile, currentUnit.unitBaseData.classBaseStats.AttackRange);
                    CurrentCommandController = currentUnit.attackController;

                    break;
            }

            FilterOutTheUnviableTiles(SelectableTiles);
            ShowSelectableTiles(currentCommandType);
            SelectionController.SelectedUnit.OnTargetTileSelected += OnTileSelected;
        }

        //TODO figure it out a way not to use the switch twice.
        private void OnTileSelected(Tile selectedTile)
        {
            // If we clicked in a tile that is not selectable or there is a unit over it, we just bail out.
            if (!SelectableTiles.Contains(selectedTile) ||
                !CurrentCommandController.IsSelectionViable(selectedTile)) return;

            // Here we are going to create the command and send to the CommandInvoker to excute it.
            ICommand newCommand = null;
            switch (currentCommandType)
            {
                case CommandType.Move:
                    newCommand = new MoveCommand(currentUnit.movementController, currentUnit.CurrentTile, selectedTile);
                    CommandInvoker.AddCommand(newCommand);
                    break;

                case CommandType.Attack:
                    newCommand = new AttackCommand(currentUnit.attackController, selectedTile);
                    CommandInvoker.AddCommand(newCommand);
                    break;
            }
        }

        public void EndInputRead()
        {
            CurrentCommandController = null;
            ResetSearch();
            OnReadCommandEnd?.Invoke();
        }

        private void ResetSearch()
        {
            BFS.ResetSearchData(SelectableTiles);
            SelectionController.SelectedUnit.OnTargetTileSelected -= OnTileSelected;
        }

        private void FilterOutTheUnviableTiles(List<Tile> selectableTiles)
        {
            foreach (var tile in selectableTiles.ToList())
            {
                if (!CurrentCommandController.IsSelectionViable(tile))
                {
                    tile.ResetSearchData();
                    selectableTiles.Remove(tile);
                }
            }
        }

        private void ShowSelectableTiles(CommandType commandType)
        {
            foreach (var tile in selectableTiles)
            {
                tile.TileMaterialController.SetMaterialByType(commandType);
            }
        }
    }
}