
using UnityEngine;

public class FireFlower : ItemBase
{
    private bool isAte;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAte)
            return;

        if (collision.CompareTag("Player"))
        {
            isAte = true;
            collision.GetComponent<PlayerAnimation>().EatFireFlower();
            Destroy(gameObject);
        }
    }
}
