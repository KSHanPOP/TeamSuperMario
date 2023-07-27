using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Plant : MonoBehaviour, IShakeable
{
    public void Shake(Vector2 _)
    {   
        Destroy(transform.parent.gameObject);        
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

    private void OnDestroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
