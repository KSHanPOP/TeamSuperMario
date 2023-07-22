using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWall : MonoBehaviour, IGameSessionListener
{
    private EdgeCollider2D leftWall;

    private EdgeCollider2D rightWall;

    private void Awake()
    {
        var walls = new GameObject("Wall");

        walls.layer = LayerMask.NameToLayer("Platform");

        walls.transform.position = Vector3.zero;

        leftWall = walls.AddComponent<EdgeCollider2D>();
        leftWall.enabled = false;

        rightWall = walls.AddComponent<EdgeCollider2D>();
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

        var startX = 0f;
        var endX = 24 * gameData.MapRowLength;

        var startY = -5f;
        var endY = 13.5f * gameData.MapColLength;     

        List<Vector2> leftPoints = new(2);
        leftPoints.Add(new Vector2(startX, startY));
        leftPoints.Add(new Vector2(startX, endY));

        leftWall.SetPoints(leftPoints);

        List<Vector2> rightPoints = new(2);
        rightPoints.Add(new Vector2(endX, startY));
        rightPoints.Add(new Vector2(endX, endY));

        rightWall.SetPoints(rightPoints);
    }

    public void GameStop()
    {
        leftWall.enabled = false;
        rightWall.enabled = false;
    }


}
