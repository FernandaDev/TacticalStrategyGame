using UnityEngine;

[CreateAssetMenu(fileName = "Map Editor Data", menuName = "MapEditorData")]
public class MapEditorData : ScriptableObject
{
    public int gridSize = 0;
    public GameObject tilePrefab = null;
}
