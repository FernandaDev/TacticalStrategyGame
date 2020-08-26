using System;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    public static Action<ActionButton> OnActionButtonPressed;

    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(OnButtonClicked);
    }
    
    void OnButtonClicked()
    {
        OnActionButtonPressed?.Invoke(this);
    }
}
