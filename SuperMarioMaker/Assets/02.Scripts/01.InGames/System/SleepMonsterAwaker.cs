using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepMonsterAwaker : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D boxCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IAwake awake))
            awake.OnAwake();        
    }    

    public void ClearMonsters()
    {
        LayerMask mask = LayerMask.GetMask("Monster");

        var monsters = Physics2D.BoxCastAll(transform.position, boxCollider.size * 2, 0f, Vector2.zero, 0f, mask);

        int scoreCount = 0;
        foreach (var monster in monsters)
        {
            ScoreManager.Instance.GetComboScore(scoreCount++, monster.transform.position);
            Destroy(monster.collider.gameObject);
        }
    }
}
