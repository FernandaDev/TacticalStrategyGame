using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitUIDisplay : MonoBehaviour
{
    public TextMeshProUGUI unitName;
    public TextMeshProUGUI hpText;
    public Slider hpSlider;
    public CommandMenuDisplay commandMenuDisplay; //TODO do we need this?

    public void SetupDisplay(UnitData unitData)
    {
        if (unitName) unitName.text  = unitData.Unitclass.ToString();
        if (hpText)   hpText.text    = unitData.MaxHealth.ToString();
        if (hpSlider) hpSlider.value = unitData.MaxHealth;
    }
}
