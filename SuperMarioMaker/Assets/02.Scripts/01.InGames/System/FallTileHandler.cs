using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTileHandler : MonoBehaviour
{
    private float startPoint;
    private float endPoint;

    [SerializeField]
    private float posY;

    [SerializeField]
    BoxCollider2D trigger;

    public void StartTest()
    {
        trigger.enabled = true;

        var gameData = GameManager.instance.gameData;

        startPoint = 0f;
        endPoint = gameData.TileX * gameData.MapRowLength;

        transform.position = new Vector3((startPoint + endPoint) * 0.5f, posY, 0f);
        trigger.size = new Vector2(endPoint - startPoint, trigger.size.y);
    }    

    public void StopTest()
    {
        trigger.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            SoundManager.Instance.StopAll();
            SoundManager.Instance.PlaySFX("Die");
            Invoke(nameof(GameOver), 3f);
        }

        Destroy(collision.gameObject);        
    }

    private void GameOver()
    {
        if (SceneLoader.Instance.State == SceneState.Tool)
            ToolManager.Instance.GoTool();
        else
        {

        }
    }
}
