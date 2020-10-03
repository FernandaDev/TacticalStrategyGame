using UnityEngine;
using UnityEngine.UI;

namespace FernandaDev
{
    public class CommandMenuDisplay : MonoBehaviour
    {
        [SerializeField] Transform contentTransform;
        [SerializeField] Transform confirmPanelTransform;
        [SerializeField] Button cancelButton;

        CommandButton currentSelectedCommand;

        private void Start()
        {
            foreach (Transform child in contentTransform)
            {
                var commandButtonDisplay = child.GetComponent<CommandButton>();
                commandButtonDisplay?.SetupCommandButton();
                commandButtonDisplay.OnButtonClicked += OnCommandButtonClicked;
            }

            Display(contentTransform.gameObject, false);
            Display(cancelButton.gameObject, false);
            Display(confirmPanelTransform.gameObject, false);

            cancelButton.onClick.AddListener(OnCancelCommand);
        }

        public void OnConfirmingCommand()
        {
            Display(confirmPanelTransform.gameObject, true);
        }
        public void ShowMenu(bool showMenu) => Display(contentTransform.gameObject, showMenu);
        void Display(GameObject objectToDisplay, bool shouldDisplay)
        {
            objectToDisplay.SetActive(shouldDisplay);

        }

        public void OnCommandTileSelected()
        {
            Display(cancelButton.gameObject, false);
            currentSelectedCommand = null;
        }

        void OnCommandButtonClicked(CommandButton clickedActionButton)
        {
            Display(contentTransform.gameObject, false);
            Display(cancelButton.gameObject, true);
            currentSelectedCommand = clickedActionButton;
        }

        void OnCancelCommand()
        {
            Display(contentTransform.gameObject, true);
            Display(cancelButton.gameObject, false);

            currentSelectedCommand.OnActionCancel();
        }

        private void OnDestroy()
        {
            foreach (Transform child in contentTransform)
                child.GetComponent<CommandButton>().OnButtonClicked -= OnCommandButtonClicked;

            cancelButton.onClick.RemoveListener(OnCancelCommand);
        }

        #region OUTSIDE DEPENDECES

        public void OnConfirmButton()
        {
            CommandInvoker.OnCommandConfirmationResponse(true);
            Display(confirmPanelTransform.gameObject, false);
            ShowMenu(true);
        }

        public void OnCancelButton()
        {
            CommandInvoker.OnCommandConfirmationResponse(false);
            Display(confirmPanelTransform.gameObject, false);
            ShowMenu(true);
        }

        #endregion
    }
}