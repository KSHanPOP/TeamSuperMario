using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private float moveTime = 2f;

    private float timer = 0f;    

    private float dir = 1;

    private bool isWork = true;

    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();        
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            PlayerState.Instance.transform.parent.GetComponentInChildren<Animator>().SetTrigger("UsingWarpZone");
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            isWork = !isWork;
        }

        if (!isWork)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        timer += Time.deltaTime;

        if(timer > moveTime)
        {
            timer = 0f;

            dir = -dir;
        }        

        rb.velocity = new Vector2(0, dir * speed);
    }

}
