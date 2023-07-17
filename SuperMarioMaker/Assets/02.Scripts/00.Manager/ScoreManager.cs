using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int[] scoreCombo;

    public static ScoreManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void GetScore(int score)
    {
        if (SceneLoader.Instance.State == SceneState.Tool)
        {   
            ToolManager.Instance.AddScore(score);
        }
    }

    public void GetCoin(int count)
    {
        if (SceneLoader.Instance.State == SceneState.Tool)
        {
            ToolManager.Instance.AddCoin(count);
            ToolManager.Instance.AddScore(count * 100);
        }
    }
}
