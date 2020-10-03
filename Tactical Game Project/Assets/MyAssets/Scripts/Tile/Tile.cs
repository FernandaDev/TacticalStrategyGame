using System.Collections.Generic;
using UnityEngine;

namespace FernandaDev
{
    public class Tile : MonoBehaviour
    {
        [HideInInspector] [SerializeField] bool isWalkable;
        [HideInInspector] [SerializeField] int xCoord, yCoord;

        public Unit UnitOverTile { get; set; }

        public List<Tile> Neighbours = new List<Tile>();
        public SearchData searchData = new SearchData();

        private TileMaterialController tileMaterialController;
        public TileMaterialController TileMaterialController
        {
            get
            {
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
            if (UnitOverTile != null) return true;

            return false;
        }

        public void SetTileCoords(int x, int y)
        {
            xCoord = x;
            yCoord = y;
        }

        public void FindNeighbours()
        {
            if (xCoord > 0)
                Neighbours.Add(Map.Instance.GetTileAt(xCoord - 1, yCoord));

            if (xCoord < MapEditorSettings.GridSize - 1)
                Neighbours.Add(Map.Instance.GetTileAt(xCoord + 1, yCoord));

            if (yCoord > 0)
                Neighbours.Add(Map.Instance.GetTileAt(xCoord, yCoord - 1));

            if (yCoord < MapEditorSettings.GridSize - 1)
                Neighbours.Add(Map.Instance.GetTileAt(xCoord, yCoord + 1));
        }

        public void ResetSearchData()
        {
            searchData.distanceToStartPoint = 0;
            searchData.IsVisited = false;
            searchData.parent = null;
            TileMaterialController.SetMaterialByType(MaterialType.Default);
        }
    }

    public class SearchData
    {
        public Tile parent { get; set; }
        public bool IsVisited { get; set; }
        public int distanceToStartPoint { get; set; }
    }
}