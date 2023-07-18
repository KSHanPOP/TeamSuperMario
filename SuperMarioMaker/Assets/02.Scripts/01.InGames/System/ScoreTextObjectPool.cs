using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ScoreTextObjectPool : MonoBehaviour
{
    private IObjectPool<ScoreText> pool;

    [SerializeField]
    private ScoreText prefab;

    private void Awake()
    {
        pool = new ObjectPool<ScoreText>
            (
            Create,
            Get,
            Release,
            Destroy,
            maxSize: 128
            );
    }
    private ScoreText Create()
    {
        ScoreText scoreText = Instantiate(prefab);
        scoreText.SetPool(pool);

        return scoreText;
    }
    private void Get(ScoreText scoreText)
    {
        scoreText.gameObject.SetActive(true);
    }
    private void Release(ScoreText scoreText)
    {
        scoreText.gameObject.SetActive(false);
    }
    private void Destroy(ScoreText scoreText)
    {
        Destroy(scoreText.gameObject);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            pool.Get();
        }
    }
}
