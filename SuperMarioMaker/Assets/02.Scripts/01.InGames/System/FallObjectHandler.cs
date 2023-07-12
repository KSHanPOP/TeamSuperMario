using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallObjectHandler : MonoBehaviour
{
    private float startPoint;
    private float endPoint;

    [SerializeField]
    private float height;

    [SerializeField]
    BoxCollider2D trigger;

    public void Init(float startPoint, float endPoint)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;

        transform.position = new Vector3((startPoint + endPoint) * 0.5f, height, 0f);
        trigger.size = new Vector2(endPoint - startPoint, height);
    }    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Logger.Debug("game over");
        }

        Destroy(collision.gameObject);        
    }
}
