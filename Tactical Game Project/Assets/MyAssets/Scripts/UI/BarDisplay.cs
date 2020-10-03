using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace FernandaDev
{
    public class BarDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI textValue;
        [SerializeField] Slider valueSlider;

        float _currentValue;

        public void SetupValues(float maxValue)
        {
            _currentValue = maxValue;
            valueSlider.maxValue = maxValue;

            DisplayValues();
        }

        public void RefreshValues(float newValue)
        {
            _currentValue = newValue;
            DisplayValues();
        }

        void DisplayValues()
        {
            textValue.text = _currentValue.ToString() + "/" + valueSlider.maxValue.ToString();
            valueSlider.value = _currentValue;
        }
    }
}