using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWall : MonoBehaviour, IGameSessionListener
{
    private BoxCollider2D leftWall;

    private BoxCollider2D rightWall;

    private void Awake()
    {
        leftWall = gameObject.AddComponent<BoxCollider2D>();
        leftWall.enabled = false;

        rightWall = gameObject.AddComponent<BoxCollider2D>();
        rightWall.enabled = false;
    }

    public void GameStart()
    {
        leftWall.enabled = true;
        rightWall.enabled = true;

        if (GameManager.instance == null)
            return;

        GameData gameData = null;

        if (SceneLoader.Instance.State == SceneState.Tool)
        {
            gameData = GameManager.instance.gameData;
        }
        else if (SceneLoader.Instance.State == SceneState.MainGame)
        {
            gameData = InGameManager.Instance.GameData;
        }

        var startPoint = 0f;
        var endPoint = 24 * gameData.MapRowLength;

        //transform.position = new Vector3((startPoint + endPoint) * 0.5f, posY, 0f);
        //trigger.size = new Vector2(endPoint - startPoint, trigger.size.y);
    }

    public void GameStop()
    {
        leftWall.enabled = false;
        rightWall.enabled = false;
    }


}
