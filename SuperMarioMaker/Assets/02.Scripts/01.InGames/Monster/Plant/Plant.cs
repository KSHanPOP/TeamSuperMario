using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, IShakeable
{
    public void Shake(Vector2 _)
    {
        Logger.Debug("plant shaked");
        Destroy(transform.parent.gameObject, 0.1f);        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<CollisionMonster>().Hit();
        }
    }
}
