using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Button moveButton;
    [SerializeField] Transform menuTransform;

    CommandInputReader commandInputReader;

    private void Awake()
    {
        commandInputReader = GetComponentInParent<CommandInputReader>();
    }

    private void Start()
    {
        ShowMenu(false);

        moveButton.onClick.AddListener(() => { commandInputReader.ReadInputCommand(CommandType.Move); ShowMenu(false); });
    }

    public void ShowMenu(bool showMenu)
    {
        menuTransform.gameObject.SetActive(showMenu);
    }
}
