using UnityEngine;
using UnityEngine.UI;

public class FindListenerDebug : MonoBehaviour
{
    // �� ��ũ��Ʈ�� ����� ��ư
    public Button targetButton;

    private void Start()
    {
        // ��ư�� onClick �̺�Ʈ�� �����ʸ� �߰�
        targetButton.onClick.AddListener(FindListeners);
    }

    public void FindListeners()
    {
        Logger.Debug("searching listneres");
        // ����� ��� �����ʵ��� ���� ���
        UnityEngine.Events.UnityEvent onClickEvent = targetButton.onClick;
        int listenerCount = onClickEvent.GetPersistentEventCount();
        for (int i = 0; i < listenerCount; i++)
        {
            Object targetObject = onClickEvent.GetPersistentTarget(i);
            string methodName = onClickEvent.GetPersistentMethodName(i);
            Logger.Debug("Listener #" + i + ": " + targetObject.name + "." + methodName);
        }
    }
}
