using System.Collections.Generic;
using UnityEngine;

public static class BFS
{
    public static List<Tile> SearchTiles(Tile startTile, int searchDistance)
    {
        Queue<Tile> tileSearchQueue = new Queue<Tile>();
        List<Tile> processedTiles = new List<Tile>();

        tileSearchQueue.Enqueue(startTile);

        startTile.searchData.IsVisited = true;

        while(tileSearchQueue.Count > 0)
        {
            Tile processingTile = tileSearchQueue.Dequeue();

            if (processingTile.IsWalkable)
            {
                processedTiles.Add(processingTile);

                if (processingTile.searchData.distanceToStartPoint < searchDistance)
                {
                    foreach (Tile neighbour in processingTile.Neighbours)
                    {
                        if(!neighbour.searchData.IsVisited)
                        {
                            neighbour.parent = processingTile;
                            neighbour.searchData.IsVisited = true;
                            neighbour.searchData.distanceToStartPoint = 1 + processingTile.searchData.distanceToStartPoint;

                            tileSearchQueue.Enqueue(neighbour);
                        }
                    }
                }
            }
        }

        return processedTiles;
    }

    //public static List<Tile> SearchTiles(Tile startTile, int searchDistance, bool ignoreTilesWithUnit)
    //{
    //    Queue<Tile> tileSearchQueue = new Queue<Tile>();
    //    List<Tile> processedTiles = new List<Tile>();

    //    tileSearchQueue.Enqueue(startTile);

    //    startTile.searchData.IsVisited = true;

    //    while (tileSearchQueue.Count > 0)
    //    {
    //        Tile processingTile = tileSearchQueue.Dequeue();

    //        if (processingTile.IsWalkable)
    //        {
    //            processedTiles.Add(processingTile);

    //            if (processingTile.searchData.distanceToStartPoint < searchDistance)
    //            {
    //                foreach (Tile neighbour in processingTile.Neighbours)
    //                {
    //                    if (!neighbour.searchData.IsVisited)
    //                    {
    //                        neighbour.parent = processingTile;
    //                        neighbour.searchData.IsVisited = true;
    //                        neighbour.searchData.distanceToStartPoint = 1 + processingTile.searchData.distanceToStartPoint;

    //                        tileSearchQueue.Enqueue(neighbour);
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    if (ignoreTilesWithUnit)
    //    {
    //        foreach (var tile in processedTiles.ToList())
    //        {
    //            if (tile.HasUnitOver())
    //            {
    //                tile.ResetSearchData();
    //                processedTiles.Remove(tile);
    //            }
    //        }
    //    }
        
    //    return processedTiles;
    //}

    public static void ResetSearchData(List<Tile> tilesToReset)
    {
        if (tilesToReset == null) return;

        foreach (var tile in tilesToReset)
        {
            tile.ResetSearchData();
            tile.parent = null;
        }
        tilesToReset.Clear();
    }
}
