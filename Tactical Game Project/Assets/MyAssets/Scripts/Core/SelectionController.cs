using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FernandaDev
{
    public class SelectionController : MonoBehaviour
    {
        public static SelectionController Instance { get; private set; }

        private static PlayerUnit selectedUnit;
        public static PlayerUnit SelectedUnit
        {
            get => selectedUnit;
            private set
            {
                if (selectedUnit != value)
                {
                    if (selectedUnit != null) selectedUnit.OnGetDeselected(); // deselect the old unit
                    selectedUnit = value;
                    if (selectedUnit != null) selectedUnit.OnGetSelected(); // select the new unit
                }
            }
        }

        private Tile hoveredTile = null;
        public Tile HoveredTile
        {
            get => hoveredTile;
            private set
            {
                if (hoveredTile != value)
                {
                    hoveredTile = value;
                    DisplayHoverPreview(value);
                    OnHoverTileChangedCallback?.Invoke(value);
                }
            }
        }

        SelectionState selectionState = SelectionState.UnitSelection;
        public SelectionState SelectionState => selectionState;

        public event Action<Tile> OnHoverTileChangedCallback;

        [SerializeField] List<Tile> previewHoveredTilesList = new List<Tile>();
        private Camera _cam;
        private bool canSelect = true;
        private bool canHover = false;
        private CommandInputReader commandInputReader;

        private void OnEnable()
        {
            CommandInputReader.OnReadCommandStart += OnReadCommandStart;
            CommandInputReader.OnReadCommandEnd += OnReadCommandEnd;
        }
        private void OnDisable()
        {
            CommandInputReader.OnReadCommandStart -= OnReadCommandStart;
            CommandInputReader.OnReadCommandEnd -= OnReadCommandEnd;
        }

        void Awake()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(this);

            _cam = Camera.main;
            if (!_cam) FindObjectOfType<Camera>();

            commandInputReader = CommandInputReader.Instance;
        }

        void OnReadCommandStart()
        {
            canHover = true;
            selectionState = SelectionState.CommandTileSelection;
        }

        void OnReadCommandEnd()
        {
            canHover = false;
            ResetHoverPreview();
            canSelect = true;
            selectionState = SelectionState.UnitSelection;
        }

        void Update()
        {
            if (!canSelect) return;

            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, Layers.TILE_LAYERMASK))
            {
                if (canHover) HoveredTile = hitInfo.collider.GetComponent<Tile>();

                if (Input.GetMouseButtonDown(0))
                {
                    if (EventSystem.current.IsPointerOverGameObject()) return;

                    HandleTileSelection(hitInfo.collider.GetComponent<Tile>());
                }
            }
        }

        void HandleTileSelection(Tile selectedTile)
        {
            switch (selectionState)
            {
                // At this state, we can select unit at any time, and if we select a tile we should deselect the last unit
                case SelectionState.UnitSelection:
                    if (selectedTile.HasUnitOver() && selectedTile.UnitOverTile is AIUnit) return; //this will prevent us to select a Enemy's unit.

                    SelectedUnit = (PlayerUnit)selectedTile.UnitOverTile; //REMEMBER 'SelectedUnit' can be NULL and it will handle the selection and deselection of the unit

                    break;

                case SelectionState.CommandTileSelection:
                    if (CommandInputReader.Instance.SelectableTiles.Contains(selectedTile))
                    {
                        selectedUnit.OnCommandTileSelected(selectedTile);
                        canSelect = false;
                    }
                    break;
            }
        }

        private void DisplayHoverPreview(Tile targetTile)
        {
            if (!commandInputReader.SelectableTiles.Contains(targetTile)) return;

            if (previewHoveredTilesList.Count > 0) ResetHoverPreview();

            targetTile.TileMaterialController.SetMaterialByType(MaterialType.Target);
            previewHoveredTilesList.Add(targetTile);

            if (targetTile.searchData.parent != null)
            {
                var currentTileToCheck = targetTile.searchData.parent;

                while (currentTileToCheck != null && commandInputReader.SelectableTiles.Contains(currentTileToCheck))
                {
                    currentTileToCheck.TileMaterialController.SetMaterialByType(MaterialType.Hover);

                    previewHoveredTilesList.Add(currentTileToCheck);

                    currentTileToCheck = currentTileToCheck.searchData.parent;
                }
            }
        }

        private void ResetHoverPreview()
        {
            foreach (var tile in previewHoveredTilesList)
            {
                if (commandInputReader.CurrentCommandController != null)
                    tile.TileMaterialController.SetMaterialByType(commandInputReader.currentCommandType);
                else
                    tile.TileMaterialController.SetMaterialByType(MaterialType.Default);
            }
            previewHoveredTilesList.Clear();
        }
    }
}