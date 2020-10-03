using System;
using UnityEngine;

namespace FernandaDev
{
    public class Unit : MonoBehaviour
    {
        public UnitBaseData unitBaseData;
        public Tile CurrentTile { get; private set; }

        public static event Action<Unit> OnUnitSelected;
        public static event Action<Unit> OnUnitDeselected;

        public event Action<Tile> OnTargetTileSelected;

        public MovementController movementController { get; private set; } //TODO  we need to change this later
        public AttackController attackController { get; private set; }

        protected virtual void Awake()
        {
            movementController = GetComponent<MovementController>();
            attackController = GetComponent<AttackController>();
        }

        protected virtual void Start()
        {
            UpdateCurrentTile();
        }

        public void UpdateCurrentTile()
        {
            if (CurrentTile)
                CurrentTile.UnitOverTile = null; // before changing the new tile, we should reset the old one.

            CurrentTile = Map.Instance.GetCurrentTileFromWorld(transform.position);
            CurrentTile.UnitOverTile = this;
        }

        public virtual void OnGetSelected() => OnUnitSelected?.Invoke(this);
        public virtual void OnGetDeselected() => OnUnitDeselected?.Invoke(this);
        public virtual void OnCommandTileSelected(Tile selectedTile) => OnTargetTileSelected?.Invoke(selectedTile);
    }
}