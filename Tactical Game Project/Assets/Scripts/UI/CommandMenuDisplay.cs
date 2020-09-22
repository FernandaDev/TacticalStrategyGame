using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandMenuDisplay : MonoBehaviour
{
    [SerializeField] Transform contentTransform;
    [SerializeField] Button cancelButton;
    List<ActionButtonDisplay> actionButtons = new List<ActionButtonDisplay>();

    private void Start()
    {
        foreach (Transform child in contentTransform)
        {
            var actionButtonDisplay = child.GetComponent<ActionButtonDisplay>();

            if (actionButtonDisplay)
            {
                actionButtonDisplay.SetupActionButton();
                actionButtons.Add(actionButtonDisplay);
            }
        }

        ShowMenu(false);

        ActionButtonDisplay.OnAnyButtonClicked += OnActionButtonClicked;
    }

    void OnActionButtonClicked()
    {
        ShowMenu(false);
    }

    public void ShowMenu(bool showMenu)
    {
        contentTransform.gameObject.SetActive(showMenu);
    }

    private void OnDestroy()
    {
        ActionButtonDisplay.OnAnyButtonClicked -= OnActionButtonClicked;
    }
}
