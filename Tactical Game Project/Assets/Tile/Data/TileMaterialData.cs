using UnityEngine;

[CreateAssetMenu(fileName = "Tile Material Data", menuName = "ScriptableObjects/Tile Material Data")]
public class TileMaterialData : ScriptableObject
{
    [Header("Materials")]

    [SerializeField] Material walkableMaterial;
    public Material WalkableMaterial { get { return walkableMaterial; } }

    [SerializeField] Material unwalkableMaterial;
    public Material UnwalkableMaterial { get { return unwalkableMaterial; } }

    [SerializeField] Material moveMaterial;
    public Material MoveMaterial { get { return moveMaterial; } }

    [SerializeField] Material attackMaterial;
    public Material AttackMaterial { get { return attackMaterial; } }

    [SerializeField] Material skillMaterial;
    public Material SkillMaterial { get { return skillMaterial; } }

}
