using UnityEngine;
public class Mushroom : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var go = collision.gameObject;

        if (go.CompareTag("Player"))
        {
            go.GetComponent<PlayerAnimation>().EatMushroom();
            Destroy(gameObject);
        }
    }
}
