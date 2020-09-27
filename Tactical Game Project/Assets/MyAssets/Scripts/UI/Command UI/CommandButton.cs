using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class CommandButton : MonoBehaviour
{
    public CommandType commandType;
    public Button button;
    public event Action<CommandButton> OnButtonClicked;

    public virtual void SetupCommandButton()
    {
        if (!button)
        {
            Debug.LogError("Please assign a button.");
            return;
        }

        button.onClick.AddListener(Click);

        if (GetComponentInChildren<TextMeshProUGUI>())
            GetComponentInChildren<TextMeshProUGUI>().text = commandType.ToString();
    }

    void Click()
    { 
        CommandInputReader.Instance.ReadInput(commandType);
        OnButtonClicked?.Invoke(this);
    }

    public virtual void OnActionCancel() => CommandInputReader.Instance.EndInputRead();
}
