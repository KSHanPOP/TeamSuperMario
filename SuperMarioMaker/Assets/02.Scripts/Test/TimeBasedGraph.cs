using UnityEngine;

public class TimeBasedGraph : MonoBehaviour
{
    public float graphWidth = 10f;   // 그래프의 가로 길이
    public float graphHeight = 5f;  // 그래프의 세로 길이
    public int resolution = 100;    // 그래프 해상도(점의 개수)

    private LineRenderer lineRenderer;
    private Vector3[] graphPoints;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = resolution;
        graphPoints = new Vector3[resolution];
    }

    private void Update()
    {
        // 그래프 포인트 업데이트
        for (int i = 0; i < resolution; i++)
        {
            float t = (float)i / (resolution - 1);
            float x = Mathf.Lerp(-graphWidth / 2f, graphWidth / 2f, t);   // x값은 그래프의 가로 길이에 비례하여 변화
            float y = Mathf.Sin(Time.time + t * Mathf.PI * 4f) * graphHeight / 2f;    // 예시로 사인 함수를 사용하여 y값을 계산
            graphPoints[i] = new Vector3(x, y, 0f);
        }

        // Line Renderer에 그래프 포인트를 설정하여 그래프 업데이트
        lineRenderer.SetPositions(graphPoints);
    }
}
