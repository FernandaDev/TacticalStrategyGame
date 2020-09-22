using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHandler : MonoBehaviour
{
    public static MouseHandler Instance { get; private set; }

    private static Unit selectedUnit;
    public static Unit SelectedUnit
    {
        get => selectedUnit;
        private set
        {
            if (selectedUnit != value)
            {
                if (selectedUnit != null) selectedUnit.OnDeselected(); // deselect the old unit
                selectedUnit = value;
                if (selectedUnit != null) selectedUnit.OnSelected(); // select the new unit
            }
        }
    }

    SelectionState selectionState = SelectionState.UnitSelection;
    public SelectionState SelectionState => selectionState;

    private Tile hoveredTile = null;
    public Tile HoveredTile
    {
        get => hoveredTile;
        private set
        {
            if (value != hoveredTile)
            {
                hoveredTile = value;
                OnHoverTileChangedCallback?.Invoke(hoveredTile);
            }
        }
    }

    public event Action<Tile> OnHoverTileChangedCallback;
    public event Action<Tile> OnTileSelected;

    Camera _cam;
    bool canSelect = true;
    bool canHover = false;

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
        if (!_cam) GameObject.FindObjectOfType<Camera>();
    }

    void Update()
    {
        if (!canSelect) return;

        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, Layers.TILE_LAYERMASK))
        {
            if (canHover && hitInfo.collider.gameObject != hoveredTile?.gameObject)
                HoveredTile = hitInfo.collider.GetComponent<Tile>();

            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;

                HandleTileSelection(hitInfo.collider.GetComponent<Tile>());
            }
        }
    }

    void OnReadCommandStart()
    {
        canHover = true;
        selectionState = SelectionState.CommandTileSelection;
    }
    void OnReadCommandEnd()
    {
        canSelect = true;
        canHover = false;
        SelectedUnit = null;
        selectionState = SelectionState.UnitSelection;
    }

    void HandleTileSelection(Tile selectedTile)
    {
        switch (selectionState)
        {
            // At this state, we can select unit at any time, and if we select a tile we should deselect the last unit
            case SelectionState.UnitSelection:
                SelectedUnit = selectedTile.UnitOverTile; //REMEMBER 'SelectedUnit' can be NULL and it will handle the selection and deselection of the unit

                break;

            case SelectionState.CommandTileSelection:
                if (CommandInputReader.Instance.SelectableTiles.Contains(selectedTile) &&
                    CommandInputReader.Instance.CurrentCommandController.IsSelectionViable(selectedTile))
                {
                    OnTileSelected?.Invoke(selectedTile);
                    canSelect = false;
                }
                break;
        }
    }
}