using UnityEngine;

public class TimeBasedGraph : MonoBehaviour
{
    public float graphWidth = 10f;   // �׷����� ���� ����
    public float graphHeight = 5f;  // �׷����� ���� ����
    public int resolution = 100;    // �׷��� �ػ�(���� ����)

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
        // �׷��� ����Ʈ ������Ʈ
        for (int i = 0; i < resolution; i++)
        {
            float t = (float)i / (resolution - 1);
            float x = Mathf.Lerp(-graphWidth / 2f, graphWidth / 2f, t);   // x���� �׷����� ���� ���̿� ����Ͽ� ��ȭ
            float y = Mathf.Sin(Time.time + t * Mathf.PI * 4f) * graphHeight / 2f;    // ���÷� ���� �Լ��� ����Ͽ� y���� ���
            graphPoints[i] = new Vector3(x, y, 0f);
        }

        // Line Renderer�� �׷��� ����Ʈ�� �����Ͽ� �׷��� ������Ʈ
        lineRenderer.SetPositions(graphPoints);
    }
}
