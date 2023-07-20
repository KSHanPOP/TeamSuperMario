using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Net.NetworkInformation;

public class SpeedGraphDrawer : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private Coroutine speedTrackCoroutine;

    [SerializeField]
    private TextMeshPro textMesh;

    private List<TextMeshPro> textMeshes = new List<TextMeshPro>();

    [SerializeField]
    private Transform startTransform;

    [SerializeField]
    private float graphWidthMult = 3f;

    [SerializeField]
    private float graphHeightAdjust = 0.5f;

    [SerializeField]
    private bool useCustomStartPos;

    private Vector3 startPos;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;

        startPos = startTransform.position;
    }

    public void StartTrackSpeed()
    {
        foreach (var t in textMeshes)
            Destroy(t);

        speedTrackCoroutine = StartCoroutine(SpeedTrackCoroutine());
    }

    public void StopTrackSpeed()
    {
        if (speedTrackCoroutine != null)
        {
            StopCoroutine(speedTrackCoroutine);
        }
    }

    private IEnumerator SpeedTrackCoroutine()
    {
        while(true)
        {
            if(Input.GetKeyDown(KeyCode.D))
            {
                break;
            }
            yield return null;
        }        
        
        Rigidbody2D playerBody = PlayerState.Instance.transform.parent.GetComponent<Rigidbody2D>();

        var speedTextAbovePlayer = Instantiate(textMesh, playerBody.transform);
        speedTextAbovePlayer.transform.localPosition = Vector2.up;
        speedTextAbovePlayer.sortingOrder = 10;

        if (!useCustomStartPos)
            startPos = playerBody.position + Vector2.right;

        float timer = 0f;
        lineRenderer.positionCount = 0;        

        List<Vector3> positions = new List<Vector3>();

        float speed = 0f;

        bool maxSpeedTimeChecked = false;

        while (true)
        {
            speed = playerBody.velocity.x;

            speedTextAbovePlayer.text = speed.ToString("F2");

            Vector3 pos = startPos + new Vector3(timer * graphWidthMult, speed * graphHeightAdjust);   
            positions.Add(pos);

            if (!maxSpeedTimeChecked && speed == 10f)
            {
                var text = Instantiate(textMesh);
                textMeshes.Add(text);
                text.transform.position = pos;
                text.sortingOrder = 10;
                text.text = timer.ToString("F2");

                maxSpeedTimeChecked = true;
            }

            ++lineRenderer.positionCount;
            lineRenderer.SetPositions(positions.ToArray());            

            timer+= Time.deltaTime;
            yield return null;
        }        
    }
}
