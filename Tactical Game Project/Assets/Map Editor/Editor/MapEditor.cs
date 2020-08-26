using UnityEditor;
using UnityEngine;

public class MapEditor : EditorWindow
{
    [MenuItem("MapEditor/Map Editor Tool")]
    public static void ShowWindow()
    {
        GetWindow(typeof(MapEditor));
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();

        MapEditorSettings.GridSize = EditorGUILayout.IntField("Grid Size", MapEditorSettings.GridSize);
        MapEditorSettings.TilePrefab = EditorGUILayout.ObjectField("Tile Prefab", MapEditorSettings.TilePrefab, typeof(GameObject), false) as GameObject;

        var selectedObjects = Selection.gameObjects;

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();

        EditorGUI.BeginDisabledGroup(MapEditorSettings.GridSize <= 0 || MapEditorSettings.TilePrefab == null);
        if (GUILayout.Button("Generate Map"))
        {
            GenerateMap();
        }
        EditorGUI.EndDisabledGroup();

        EditorGUI.BeginDisabledGroup(MapEditorSettings.Map == null || MapEditorSettings.Map.tilesList.Count < 1);
        if (GUILayout.Button("Erase Map"))
        {
            EraseMap();
        }
        EditorGUI.EndDisabledGroup();

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        EditorGUI.BeginDisabledGroup(selectedObjects.Length < 1 || MapEditorSettings.Map == null || MapEditorSettings.Map.tilesList.Count < 1);
        if (GUILayout.Button("Set Walkable"))
        {
            SetWalkable(selectedObjects, true);
        }

        if (GUILayout.Button("Set Unwalkable"))
        {
            SetWalkable(selectedObjects, false);
        }
        EditorGUI.EndDisabledGroup();

        GUILayout.EndHorizontal();
        EditorGUI.EndDisabledGroup();

        if (MapEditorSettings.GridSize <= 0)
        {
            EditorGUILayout.HelpBox("The grid size must be greater than 0.", MessageType.Warning);
        }
        if (MapEditorSettings.TilePrefab == null)
        {
            EditorGUILayout.HelpBox("Assign the tile prefab.", MessageType.Warning);
        }

        EditorUtility.SetDirty(MapEditorSettings.Data);
    }

    public void GenerateMap()
    {
        if (MapEditorSettings.Map != null && MapEditorSettings.Map.tilesList.Count > 0)
            EraseMap();

        if(MapEditorSettings.Map == null)
        {
            MapEditorSettings.Map = new GameObject("Map").AddComponent<Map>();
        }

        for (int x = 0; x < MapEditorSettings.GridSize; x++)
        {
            for (int y = 0; y < MapEditorSettings.GridSize; y++)
            {
                CreateTile(x, y);
            }
        }

        EditorUtility.SetDirty(MapEditorSettings.Map);
    }

    private void CreateTile(int x, int y)
    {
        var tile = Instantiate(MapEditorSettings.TilePrefab);
        tile.transform.parent = MapEditorSettings.Map.transform;
        tile.name = $"Tile {x} , {y}";
        tile.transform.position = -new Vector3(1, 0, 1) * (MapEditorSettings.GridSize - 1) * 0.5f + new Vector3(x, 0, y);
        
        var t = tile.GetComponent<Tile>();
        if (t != null)
        {
            t.SetTileCoords(x, y);
            t.IsWalkable = false;
        }
        MapEditorSettings.Map.tilesList.Add(tile.gameObject);
    }

    public void EraseMap()
    {
        if (MapEditorSettings.Map.tilesList.Count > 0)
        {
            foreach (var tile in MapEditorSettings.Map.tilesList)
            {
                DestroyImmediate(tile);
            }
            MapEditorSettings.Map.tilesList.Clear();
        }

        EditorUtility.SetDirty(MapEditorSettings.Map);
    }

    public void SetWalkable(GameObject[] selectedTiles, bool _isWalkable)
    {
        for (int i = 0; i < selectedTiles.Length; i++)
        {
            var tile = selectedTiles[i].GetComponent<Tile>();
            if (tile) tile.IsWalkable = _isWalkable;
        }
    }
}
