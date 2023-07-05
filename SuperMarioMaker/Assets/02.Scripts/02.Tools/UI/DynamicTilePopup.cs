using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DynamicTilePopup : MonoBehaviour
{
    [SerializeField]
    private Button[] buttons;

    [SerializeField]
    private Button exitButton;

    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI;

    [SerializeField]
    private Slider slider;

    //private string defaultString;

    //private void Awake()
    //{
    //    defaultString = "max " + slider.maxValue; 
    //}
    public Button GetButton(EnumItems item) => buttons[(int)item - 1];
    public Slider GetSlider() => slider;   
    public void OnSliderValueChange(float value)
    {
        textMeshProUGUI.text = ((int)value).ToString();
    }

    public void Enter(float newValue)
    {
        gameObject.SetActive(true);
        ClearListeners();
        setValue(newValue);
    }
    public void Exit()
    {   
        ClearListeners();
        gameObject.SetActive(false);
    }

    public void setValue(float newValue)
    {
        slider.value = newValue;
        textMeshProUGUI.text = ((int)slider.value).ToString();
    }
    public void ClearListeners()
    {
        foreach (var button in buttons)
            button.onClick.RemoveAllListeners();

        slider.onValueChanged.RemoveAllListeners();
    }
    private void OnDisable()
    {
        Exit();
    }
}
