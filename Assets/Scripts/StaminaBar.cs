using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Gradient _gradient;
    [SerializeField] private Image _fill;

    public void SetMaxValue(float value)
    {
        _slider.maxValue = value;
        _slider.value = value;

        _fill.color = _gradient.Evaluate(1f);
    }

    public void SetValue(float value)
    {
        _slider.value = value;

        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }
}
