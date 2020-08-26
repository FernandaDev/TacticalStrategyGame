using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField][HideInInspector] bool isWalkable;
    [SerializeField][HideInInspector] int xCoord, yCoord;
    [SerializeField][HideInInspector] List<Tile> neighbours = new List<Tile>();
    
    [HideInInspector] public List<Tile> Neighbours { get => neighbours; }
    [HideInInspector] public Tile parent;
    public SearchData searchData = new SearchData();

    private Unit unitOverTile;
    public Unit UnitOverTile => unitOverTile;

    private TileMaterialController tileMaterialController;
    public TileMaterialController TileMaterialController 
    {
        get {
            if (tileMaterialController == null)
                tileMaterialController = GetComponent<TileMaterialController>();
            return tileMaterialController; 
        }
    }

    public bool IsWalkable
    {
        get { return isWalkable; }
        set
        {
            isWalkable = value;
            TileMaterialController.SetWalkableMaterial(value);
        }
    }

    public bool HasUnitOver()
    {
        if (unitOverTile != null) return true;
        
        return false;
    }

    public void UpdateUnitOverTile(Unit newUnit)
    {
        unitOverTile = newUnit;
    }

    public void SetTileCoords(int x, int y)
    {
        xCoord = x;
        yCoord = y;
    }

    public void FindNeighbours()
    {
        if (xCoord > 0)
            neighbours.Add(Map.Instance.GetTileAt(xCoord - 1, yCoord));

        if (xCoord < MapEditorSettings.GridSize - 1)
            neighbours.Add(Map.Instance.GetTileAt(xCoord + 1, yCoord));

        if (yCoord > 0)
            neighbours.Add(Map.Instance.GetTileAt(xCoord, yCoord - 1));

        if (yCoord < MapEditorSettings.GridSize - 1)
            neighbours.Add(Map.Instance.GetTileAt(xCoord, yCoord + 1));
    }

    public void ResetSearchData()
    {
        searchData.distanceToStartPoint = 0;
        searchData.IsVisited = false;
        parent = null;
        TileMaterialController.SetMaterialByType(CommandType.Default);
    }
}

[System.Serializable]
public class SearchData
{
    public bool IsVisited = false;
    public int distanceToStartPoint = 0;
}
