using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodShow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TextMeshProUGUI _hpText;
    public void ChangeTextFood()
    {
        Slider _slider = GetComponent<Slider>();
        float _value = _slider.value;
        if (_value % 1 == 0)
        {
            _hpText.text = $"{_slider.value.ToString("F0")}/{_slider.maxValue}";
        }
        else
        {
            _hpText.text = $"{_slider.value.ToString("F1")}/{_slider.maxValue}";

        }
    }
}
