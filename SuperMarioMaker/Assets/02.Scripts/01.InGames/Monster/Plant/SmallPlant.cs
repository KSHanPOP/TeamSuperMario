using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallPlant : MonoBehaviour, IShakeable
{
    public void Shake(Vector2 _)
    {
        Logger.Debug("plant shaked");
        Destroy(gameObject);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        collision.GetComponent<CollisionMonster>().Hit();
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<CollisionMonster>().Hit();
        }
    }
}
