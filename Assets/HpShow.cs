using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpShow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TextMeshProUGUI _hpText;
    public void ChangeTextHp()
    {
        Slider _slider = GetComponent<Slider>();
        _hpText.text = $"{_slider.value}/{_slider.maxValue}";
    }
}
