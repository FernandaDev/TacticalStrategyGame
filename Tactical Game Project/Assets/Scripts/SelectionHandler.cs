using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionHandler : MonoBehaviour
{
    private static SelectionHandler instance;
    public static SelectionHandler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<SelectionHandler>();
                if (instance == null)
                {
                    instance = new GameObject("Selection Handler").AddComponent<SelectionHandler>();
                }
            }
            return instance;
        }
    }
        
    [SerializeField] Unit selectedUnit;
    public Unit SelectedUnit
    {
        get => selectedUnit;
        private set
        {
            if (selectedUnit != value)
            {
                if (selectedUnit != null) selectedUnit.OnDeselected(); // erase the old selection
                selectedUnit = value;
                if (selectedUnit != null) selectedUnit.OnSelected(); // select the new object
            }
        }
    }

    SelectionState selectionState = SelectionState.UnitSelection;
    public SelectionState SelectionState { get => selectionState; set => selectionState = value; }

    public static event Action<Tile> OnTileSelected;

    Camera _cam;
    int currentLayerMask = Layers.TILE_LAYERMASK;
    bool canSelect = true; //TODO NOT USING AT THE MOMENT!

    void Start()
    {
        _cam = Camera.main;
        if (!_cam) GameObject.FindObjectOfType<Camera>();
    }

    void Update()
    {
        if (!canSelect) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            HandleSelection();
        }
    }

    public void HandleSelection()
    {        
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition); 
        
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, currentLayerMask)) 
        {
            HandleTileSelection(hitInfo.collider.GetComponent<Tile>());
        }
    }

    public void HandleTileSelection(Tile selectedTile)
    {
        switch (SelectionState)
        {
            // At this state, we can select unit at any time, and if we select a tile we should deselect the last unit
            case SelectionState.UnitSelection:
                SelectedUnit = selectedTile.UnitOverTile; //REMEMBER 'SelectedUnit' can be NULL and it will handle the selection and deselection of the unit

                break;

            // at this state we can't select any unit, only the available tiles
            case SelectionState.CommandTileSelection:
                OnTileSelected?.Invoke(selectedTile);

                break;
        }
    }

    public void DeselectUnit() => SelectedUnit = null;
}

public enum SelectionState
{
    UnitSelection,
    CommandTileSelection
}


