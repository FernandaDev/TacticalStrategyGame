using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapEditorSettings
{
    static MapEditorData data;
    public static MapEditorData Data
    {
        get
        {
            if (data == null)
            {
                var dataPath = AssetDatabase.FindAssets("t:MapEditorData");

                if (dataPath.Length > 1)
                {
                    Debug.Log("There shouldn't be more than one Map Editor Data.");
                    return null;
                }
                data = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(dataPath[0]), typeof(MapEditorData)) as MapEditorData;
            }
            return data;
        }
    }


    public static int GridSize
    {
        get
        {
#if UNITY_EDITOR
            return Data.gridSize;
#endif
        }

        set
        {
#if UNITY_EDITOR
            Data.gridSize = value;
#endif
        }
    }

    public static GameObject TilePrefab
    {
        get
        {
#if UNITY_EDITOR
            if (data.tilePrefab == null) return null;
            return Data.tilePrefab;
#else
            return null;
#endif
        }

        set
        {
#if UNITY_EDITOR
            data.tilePrefab = value;
#endif
        }
    }

    static Map map;
    public static Map Map
    {
        get
        {
            if (map == null)
            {
                map = GameObject.FindObjectOfType<Map>();
            }

            return map;
        }
        set
        {
            map = value;
        }
    }
}


