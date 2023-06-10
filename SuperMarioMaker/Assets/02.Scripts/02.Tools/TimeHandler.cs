using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

public class TimeHandler : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_TextMeshProUGUI;
    [SerializeField]
    private Slider m_Slider;

    private void Init()
    {
        int time = Mathf.RoundToInt(ToolManager.Instance.PlayTime);
        m_Slider.value = time;
        m_Slider.onValueChanged.AddListener(OnSliderValueChanged);
        SetTime(time);
    }
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnSliderValueChanged(float value)
    {
        int time = Mathf.RoundToInt(value);
        SetTime(time);
    }
    public void SetTime(int time)
    {
        ToolManager.Instance.PlayTime = time;
        m_TextMeshProUGUI.text = time.ToString();
    }
}
