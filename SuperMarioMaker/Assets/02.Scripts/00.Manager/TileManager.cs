using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance { get; private set; }

    private Transform dynamicObjHolder;
    public Transform DynamicObjHolder { get { return dynamicObjHolder; } }

    public bool IsPlaying { get; private set; } = false;

    [SerializeField]
    FallTileHandler fallTileHandler;

    [SerializeField]
    private BoxCollider2D monsterAwaker;

    private IGameSessionListener[] gameSessionsListeners;

    private void Awake()
    {
        Instance = this;

        gameSessionsListeners = GetComponents<IGameSessionListener>();
    }
    public void StartGame()
    {
        if (IsPlaying)
            return;

        IsPlaying = true;
        MovementLimmiter.instance.CharacterCanMove = true;

        dynamicObjHolder = new GameObject("DynamicObjHolder").transform;
        BaseTile.StartGame();
        PipeWarpConnector.StartGame();
        foreach (var listener in gameSessionsListeners)
        {
            listener.GameStart();
        }

        if (SceneLoader.Instance.State == SceneState.Tool)
            PopupManager.Instance.OffPopups();

        monsterAwaker.enabled = true;
    }
    public void StopGame()
    {
        if (!IsPlaying)
            return;

        IsPlaying = false;

        Destroy(dynamicObjHolder.gameObject);
        BaseTile.StopGame();
        PipeWarpConnector.StopGame();
        foreach (var listener in gameSessionsListeners)
        {
            listener.GameStop();
        }

        monsterAwaker.enabled = false;
        Camera.main.GetComponent<SleepMonsterAwaker>().enabled = false;
    }

    public void Restart()
    {
        StopGame();
        StartGame();
    }
}
