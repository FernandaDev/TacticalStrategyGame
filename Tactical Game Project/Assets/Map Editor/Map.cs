using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private static Map instance;
    public static Map Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Map>();
                if (instance == null)
                {
                    instance = new GameObject("Map").AddComponent<Map>();
                }
            }

            return instance;
        }
    }

    public List<GameObject> tilesGOList = new List<GameObject>();

    public Dictionary<GameObject, Tile> tilemap = new Dictionary<GameObject, Tile>();

    private void Awake()
    {
        StaticBatchingUtility.Combine(this.gameObject);
        
        foreach (GameObject tileGO in tilesGOList)
        {
            tilemap.Add(tileGO, tileGO.GetComponent<Tile>());
        }

        FindAllNeighbours();
    }

    public Tile GetTileAt(int xCoord, int yCoord)
    {
        int counter = 0;

        for (int x = 0; x < MapEditorSettings.GridSize; x++)
        {
            for (int y = 0; y < MapEditorSettings.GridSize; y++)
            {
                if (x == xCoord && y == yCoord)
                    return tilemap[tilesGOList[counter]];

                counter++;
            }
        }

        return null;
    }

    public Tile GetCurrentTileFromWorld(Vector3 unitPosition)
    {
        // here we need to get the percentage of the position going through 0 to 1... 0 is left, 0.5 is the middle and 1 is the right
        float percentX = (unitPosition.x + MapEditorSettings.GridSize / 2) / MapEditorSettings.GridSize;

        float percentY = (unitPosition.z + MapEditorSettings.GridSize / 2) / MapEditorSettings.GridSize;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        // and then we need to convert it to a index of the node we are getting (that is why we subtract -1)
        int x = Mathf.FloorToInt((MapEditorSettings.GridSize) * percentX);
        int y = Mathf.FloorToInt((MapEditorSettings.GridSize) * percentY);
        
        return GetTileAt(x, y);
    }

    public void FindAllNeighbours()
    {
        for (int i = 0; i < tilesGOList.Count; i++)
        {
            tilemap[tilesGOList[i]]?.FindNeighbours();
        }
    }
}
