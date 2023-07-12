using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PipePopup : TilePopup
{
    [SerializeField]
    private Toggle plantBanToggle; 

    [SerializeField]
    private Button pipeConnectorButton;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI;
    public Toggle GetToggle() => plantBanToggle;
    public Button GetButton() => pipeConnectorButton;
    public Slider GetSlider() => slider;
    public void Enter(bool value, float sliderMaxValue ,float sliderValue)
    {
        EventPopupOff.Invoke();
        gameObject.SetActive(true);
        ClearListeners();
        SetToggleValue(value);
        SetSliderValue(sliderMaxValue, sliderValue);
    }

    public override void ClearListeners()
    {
        EventPopupOff.RemoveAllListeners();
        plantBanToggle.onValueChanged.RemoveAllListeners();
        pipeConnectorButton.onClick.RemoveAllListeners();
        slider.onValueChanged.RemoveAllListeners();        
    }

    public void OffToggleInteract()
    {
        plantBanToggle.isOn = true;
        plantBanToggle.interactable = false;
    }

    public void SetToggleValue(bool value)
    {
        plantBanToggle.isOn = value;
    }
    public void SetSliderValue(float maxValue, float sliderValue)
    {
        slider.maxValue = maxValue;
        slider.value = sliderValue;
        OnSliderValueChange(sliderValue);
    }
    public void OnSliderValueChange(float value)
    {   
        textMeshProUGUI.text = ((int)slider.value).ToString();
    }
}
