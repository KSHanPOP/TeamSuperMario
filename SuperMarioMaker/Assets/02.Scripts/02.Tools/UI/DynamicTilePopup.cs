using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DynamicTilePopup : MonoBehaviour
{
    [SerializeField]
    private Toggle[] toggles;

    [SerializeField]
    private Image[] highlights;

    [SerializeField]
    private Button exitButton;

    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI;

    [SerializeField]
    private Slider slider;

    public UnityEvent offPopup; 

    public Toggle GetToggle(EnumItems type) => toggles[(int)type];
    public Toggle[] GetToggles() => toggles;
    public Slider GetSlider() => slider;   
    public void OnSliderValueChange(float value)
    {
        if (value == 0)
            OffHighlights();
        else
            OnHighlights();

        textMeshProUGUI.text = ((int)value).ToString();
    }

    public void Enter(EnumItems type, float count, bool isQuestion)
    {
        offPopup.Invoke();
        gameObject.SetActive(true);
        ClearListeners();
        slider.minValue = isQuestion ? 1f : 0f;
        SetToggleValue(type);
        SetSliderValue(count);
    }
    public void OnToggle(bool value)
    {
        if(value && slider.value == 0)
            slider.value = 1f;
    }

    public void SetToggleValue(EnumItems type)
    {
        int idx = (int)type;
        if (idx == 3)
            idx = 0;

        toggles[idx].isOn = true;
    }
    public void SetSliderValue(float newValue)
    {
        OnSliderValueChange(newValue);
        slider.value = newValue;
    }

    public void OffHighlights()
    {
        foreach(Image highlight in highlights)
        {
            highlight.enabled = false;
        }
    }

    public void OnHighlights()
    {
        foreach (Image highlight in highlights)
        {
            highlight.enabled = true;
        }
    }
    public void ClearListeners()
    {
        foreach (var toggle in toggles)
            toggle.onValueChanged.RemoveAllListeners();

        slider.onValueChanged.RemoveAllListeners();

        offPopup.RemoveAllListeners();
    }
    private void OnDisable()
    {
        offPopup.Invoke();
        ClearListeners();
    }
}
