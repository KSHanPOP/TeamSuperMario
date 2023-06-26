using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    PlayerState player;

    private void Start()
    {
        player = PlayerState.Instance;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<CollisionMonster>().Hit();
        }        
    }
}
