using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ScoreManager : MonoBehaviour
{
    public int[] scoreCombo;
    private int scoreComboLastIndex;

    public static ScoreManager Instance { get; private set; }

    [SerializeField]
    private ScoreTextObjectPool scoreTextObjectPool;
    
    private const float scoreTextHeight = 0f;

    private IObjectPool<ScoreText> scoreTextPool;

    private void Awake()
    {
        Instance = this;
        scoreTextPool = scoreTextObjectPool.Pool;
        scoreComboLastIndex = scoreCombo.Length - 1;        
    }

    public void GetComboScore(int combo, Vector2 pos, float newHeight = scoreTextHeight)
    {
        if(combo > scoreComboLastIndex)
            combo = scoreComboLastIndex;

        GetScore(scoreCombo[combo], pos, newHeight);
    }

    public void GetScore(int score, Vector2 pos, float newHeight = scoreTextHeight)
    {
        var scoreText = scoreTextPool.Get();
        scoreText.SetValue(score, pos, newHeight);

        if (SceneLoader.Instance.State == SceneState.Tool)
        {   
            ToolManager.Instance.AddScore(score);
        }
        else if(SceneLoader.Instance.State == SceneState.MainGame)
        {
            

        }

    }

    public void GetCoin(int count)
    {
        if (SceneLoader.Instance.State == SceneState.Tool)
        {
            ToolManager.Instance.AddCoin(count);
            ToolManager.Instance.AddScore(count * 200);
        }
    }
}
