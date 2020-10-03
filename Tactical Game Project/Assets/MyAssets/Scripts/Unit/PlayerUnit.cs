using UnityEngine;

namespace FernandaDev
{
    public class PlayerUnit : Unit
    {
        UnitUIDisplay unitUIDisplay;

        protected override void Awake()
        {
            base.Awake();
            unitUIDisplay = GetComponentInChildren<UnitUIDisplay>();
        }

        protected override void Start() => base.Start();

        public override void OnGetSelected()
        {
            base.OnGetSelected();
            unitUIDisplay.commandMenuDisplay.ShowMenu(true);
        }

        public override void OnGetDeselected()
        {
            base.OnGetDeselected();
            unitUIDisplay.commandMenuDisplay.ShowMenu(false);
        }

        public override void OnCommandTileSelected(Tile selectedTile)
        {
            base.OnCommandTileSelected(selectedTile);
            unitUIDisplay.commandMenuDisplay.OnCommandTileSelected();
            CommandInvoker.OnCommandConfirmationRequest += unitUIDisplay.commandMenuDisplay.OnConfirmingCommand; // PS. the event will unsubscribe all the listener when the command is done.
        }
    }
}