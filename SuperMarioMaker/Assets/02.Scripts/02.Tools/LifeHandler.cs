using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LifeHandler : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_TextMeshPro;
    [SerializeField] private Button decreaseButton;
    [SerializeField] private Button increaseButton;

    private Coroutine increaseCoroutine; // 생명 수 증가 코루틴 참조 변수
    private Coroutine decreaseCoroutine; // 생명 수 감소 코루틴 참조 변수

    [SerializeField] private float holdDuration = 1f; // 버튼을 누른 상태를 유지해야 하는 시간
    [SerializeField] private float rapidChangeSpeed = 0.2f; // Rapid change when button is held down

    private void Start()
    {
        decreaseButton.onClick.AddListener(DecreaseLife);
        increaseButton.onClick.AddListener(IncreaseLife);

        EventTrigger decreaseTrigger = decreaseButton.GetComponent<EventTrigger>();
        EventTrigger.Entry dePointerDownEntry = new EventTrigger.Entry();
        dePointerDownEntry.eventID = EventTriggerType.PointerDown;
        dePointerDownEntry.callback.AddListener((data) => { StartDecreaseCoroutine(); });
        decreaseTrigger.triggers.Add(dePointerDownEntry);

        EventTrigger.Entry dePointerUpEntry = new EventTrigger.Entry();
        dePointerUpEntry.eventID = EventTriggerType.PointerUp;
        dePointerUpEntry.callback.AddListener((data) => { StopDecreaseCoroutine(); });
        decreaseTrigger.triggers.Add(dePointerUpEntry);

        EventTrigger increaseTrigger = increaseButton.GetComponent<EventTrigger>();
        EventTrigger.Entry InPointerDownEntry = new EventTrigger.Entry();
        InPointerDownEntry.eventID = EventTriggerType.PointerDown;
        InPointerDownEntry.callback.AddListener((data) => { StartIncreaseCoroutine(); });
        increaseTrigger.triggers.Add(InPointerDownEntry);

        EventTrigger.Entry InPointerUpEntry = new EventTrigger.Entry();
        InPointerUpEntry.eventID = EventTriggerType.PointerUp;
        InPointerUpEntry.callback.AddListener((data) => { StopIncreaseCoroutine(); });
        increaseTrigger.triggers.Add(InPointerUpEntry);

        Init();
    }

    private void Init()
    {
        int life = ToolManager.Instance.PlayerLife;
        SetText(life);
    }

    private void DecreaseLife()
    {
        int nowLife = ToolManager.Instance.PlayerLife;
        if (nowLife > 1)
        {
            nowLife--;
            ToolManager.Instance.PlayerLife = nowLife;
        }
        SetText(nowLife);
    }


    private void IncreaseLife()
    {
        int nowLife = ToolManager.Instance.PlayerLife;
        if (nowLife < 99)
        {
            nowLife++;
            ToolManager.Instance.PlayerLife = nowLife;
            SetText(nowLife);
        }
    }

    private void StartDecreaseCoroutine()
    {
        if (decreaseCoroutine != null)
            StopCoroutine(decreaseCoroutine);

        decreaseCoroutine = StartCoroutine(DecreaseLifeCoroutine());
    }

    private IEnumerator DecreaseLifeCoroutine()
    {
        yield return new WaitForSeconds(holdDuration);

        while (true)
        {
            DecreaseLife();
            yield return new WaitForSeconds(rapidChangeSpeed);
        }
    }

    private void StopDecreaseCoroutine()
    {
        if (decreaseCoroutine != null)
            StopCoroutine(decreaseCoroutine);
    }

    private void StartIncreaseCoroutine()
    {
        if (increaseCoroutine != null)
            StopCoroutine(increaseCoroutine);

        increaseCoroutine = StartCoroutine(IncreaseLifeCoroutine());
    }

    private IEnumerator IncreaseLifeCoroutine()
    {
        yield return new WaitForSeconds(holdDuration);

        while (true)
        {
            IncreaseLife();
            yield return new WaitForSeconds(rapidChangeSpeed);
        }
    }

    private void StopIncreaseCoroutine()
    {
        if (increaseCoroutine != null)
            StopCoroutine(increaseCoroutine);
    }

    private void SetText(int life)
    {
        if (life > 9)
        {
            m_TextMeshPro.text = life.ToString();
        }
        else
        {
            m_TextMeshPro.text = "0" + life.ToString();
        }
    }
}