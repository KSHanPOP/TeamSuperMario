using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    private Transform playerTransform;

    private bool isPlayerCloseDistance;

    private void Start()
    {
        playerTransform = PlayerState.Instance.transform;
    }

    private void Update()
    {
        isPlayerCloseDistance = Mathf.Abs(playerTransform.position.x - transform.position.x) < 1.5f;


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<CollisionMonster>().Hit();
        }        
    }
}
