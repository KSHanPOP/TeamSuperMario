using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MiniMapHandler : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Camera miniMapCamera;



    private void Init()
    {
        SetMinimapPosition();
    }
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMinimapPosition()
    {
        tilemap.CompressBounds();
        Vector3 tilemapCenter = tilemap.cellBounds.center;
        miniMapCamera.transform.position = new Vector3(tilemapCenter.x, tilemapCenter.y, miniMapCamera.transform.position.z);

        float sizeX = tilemap.cellBounds.size.x / (2.0f * Mathf.Abs(miniMapCamera.aspect));
        float sizeY = tilemap.cellBounds.size.y / 2.0f;

        miniMapCamera.orthographicSize = Mathf.Max(sizeX, sizeY);
    }
}
