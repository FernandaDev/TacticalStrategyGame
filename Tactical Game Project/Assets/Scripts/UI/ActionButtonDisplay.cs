using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ActionButtonDisplay : MonoBehaviour
{
    public CommandType commandType;
    public Button button;
    public static event Action OnAnyButtonClicked;

    public virtual void SetupActionButton()
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

    public virtual void Click()
    { 
        CommandInputReader.Instance.ReadInput(commandType);
        
        OnAnyButtonClicked?.Invoke();
    }
}
