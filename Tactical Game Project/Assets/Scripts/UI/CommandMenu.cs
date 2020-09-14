using UnityEngine;
using UnityEngine.UI;

public class CommandMenu : MonoBehaviour
{
    [SerializeField] Button moveButton;
    [SerializeField] Transform menuTransform;

    private void Start()
    {
        ShowMenu(false);

        moveButton.onClick.AddListener(OnClickMoveButton);
    }

    void OnClickMoveButton()
    {
        InputReader.Instance.ReadInput(CommandType.Move);
        ShowMenu(false);
    }

    public void ShowMenu(bool showMenu)
    {
        menuTransform.gameObject.SetActive(showMenu);
    }

    private void OnDestroy()
    {
        moveButton.onClick.RemoveListener(OnClickMoveButton);
    }
    
}
