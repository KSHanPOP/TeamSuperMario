using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;

public class ScoreText : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private TextMeshPro textMeshPro;

    [SerializeField]
    private float transSpeed = 1f;

    [SerializeField]
    private float lifeTime = 2f;

    private IObjectPool<ScoreText> pool;

    private void Awake()
    {
        meshRenderer.sortingOrder = 99;        
    }
    private void OnEnable()
    {
        Invoke(nameof(Release), lifeTime);
    }

    public void SetValue(int score, Vector2 pos, float heightAbove)
    {
        textMeshPro.text= score.ToString();        
        transform.position = pos + new Vector2(0, heightAbove);
    }

    public void SetPool(IObjectPool<ScoreText> pool) => this.pool= pool;

    private void Update()
    {
        transform.Translate(transSpeed * Time.deltaTime * Vector2.up );
    }

    private void Release()
    {
        pool.Release(this);        
    }

}
