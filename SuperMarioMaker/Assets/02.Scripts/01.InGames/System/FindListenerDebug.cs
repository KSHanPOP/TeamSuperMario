using UnityEngine;
using UnityEngine.UI;

public class FindListenerDebug : MonoBehaviour
{
    // 이 스크립트가 연결된 버튼
    public Button targetButton;

    private void Start()
    {
        // 버튼의 onClick 이벤트에 리스너를 추가
        targetButton.onClick.AddListener(FindListeners);
    }

    public void FindListeners()
    {
        Logger.Debug("searching listneres");
        // 연결된 모든 리스너들의 정보 출력
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
