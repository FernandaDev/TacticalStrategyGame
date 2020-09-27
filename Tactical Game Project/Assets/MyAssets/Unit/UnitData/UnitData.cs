using UnityEngine;

[CreateAssetMenu(fileName ="Unit Data", menuName = "ScriptableObjects/Unit Data")]
public class UnitData : ScriptableObject
{
    public int MaxHealth;
    public int MovementDistance;

    public UnitClass Unitclass;
}
