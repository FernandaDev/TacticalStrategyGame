using UnityEngine;

namespace FernandaDev
{
    public class TileMaterialController : MonoBehaviour
    {
        [SerializeField] TileMaterialData tileMaterialData;
        [SerializeField] [HideInInspector] Renderer rend;
        [SerializeField] [HideInInspector] Material defaultMaterial;

        public Renderer Rend
        {
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
            SetMaterialByType(MaterialType.Default);
        }

        public void SetMaterialByType(MaterialType materialType)
        {
            switch (materialType)
            {
                case MaterialType.Default:
                    Rend.sharedMaterial = defaultMaterial;
                    break;
                case MaterialType.Move:
                    Rend.sharedMaterial = tileMaterialData.MoveMaterial;
                    break;
                case MaterialType.Attack:
                    Rend.sharedMaterial = tileMaterialData.AttackMaterial;
                    break;
                case MaterialType.Skill:
                    Rend.sharedMaterial = tileMaterialData.SkillMaterial;
                    break;
                case MaterialType.Hover:
                    Rend.sharedMaterial = tileMaterialData.HoverMaterial;
                    break;
                case MaterialType.Target:
                    Rend.sharedMaterial = tileMaterialData.TargetSelectionMaterial;
                    break;
            }
        }
        public void SetMaterialByType(CommandType commandType)
        {
            switch (commandType)
            {
                case CommandType.Move:
                    Rend.sharedMaterial = tileMaterialData.MoveMaterial;
                    break;
                case CommandType.Attack:
                    Rend.sharedMaterial = tileMaterialData.AttackMaterial;
                    break;
                case CommandType.Skill:
                    Rend.sharedMaterial = tileMaterialData.SkillMaterial;
                    break;
                default:
                    break;
            }
        }
    }
}