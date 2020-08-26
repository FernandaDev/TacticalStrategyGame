using UnityEngine;

[CreateAssetMenu(fileName ="Unit Data", menuName = "ScriptableObjects/Unit Data")]
public class UnitData : ScriptableObject
{
    [SerializeField] int movementDistance;
    public int MovementDistance => movementDistance;
}
