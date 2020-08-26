using UnityEngine;

public class TileMaterialController : MonoBehaviour
{
    [SerializeField] TileMaterialData tileMaterialData;
    [SerializeField] [HideInInspector] Renderer rend;
    [SerializeField] [HideInInspector] Material defaultMaterial;

    public Renderer Rend { 
        get 
        {
            if (rend == null)
                rend = GetComponent<Renderer>();
            return rend; 
        }
    }

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    public void SetWalkableMaterial(bool isWalkable)
    {
        defaultMaterial = isWalkable ? tileMaterialData.WalkableMaterial : tileMaterialData.UnwalkableMaterial;
        SetMaterialByType(CommandType.Default);
    }

    public void SetMaterialByType(CommandType materialType)
    {
        switch (materialType)
        {
            case CommandType.Default:
                Rend.sharedMaterial = defaultMaterial;
                break;
            case CommandType.Move:
                Rend.sharedMaterial = tileMaterialData.MoveMaterial;
                break;
            case CommandType.Attack:
                Rend.sharedMaterial = tileMaterialData.AttackMaterial;
                break;
            case CommandType.Skill:
                Rend.sharedMaterial = tileMaterialData.SkillMaterial;
                break;
        }
    }
}
