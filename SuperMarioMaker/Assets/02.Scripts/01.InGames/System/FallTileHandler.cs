using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTileHandler : MonoBehaviour, IGameSessionListener
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

        if (GameManager.instance == null)
            return;

        GameData gameData = new GameData();

        if (SceneLoader.Instance.State == SceneState.Tool)
        {
            gameData = GameManager.instance.gameData;
        }
        else if (SceneLoader.Instance.State == SceneState.MainGame)
        {
            gameData = InGameManager.Instance.GameData;
        }        

        startPoint = 0f;
        endPoint = 24 * gameData.MapRowLength;

        transform.position = new Vector3((startPoint + endPoint) * 0.5f, posY, 0f);
        trigger.size = new Vector2(endPoint - startPoint, trigger.size.y);
    }

    public void StopTest()
    {
        trigger.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameOver();

            trigger.enabled = false;
            return;
        }

        Destroy(collision.gameObject);
    }

    private void GameOver()
    {
        PlayerState.Instance.CurrState.Die();
    }

    public void GameStart()
    {
        StartTest();
    }

    public void GameStop()
    {
        StopTest();
    }
}
