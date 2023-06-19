using UnityEngine;
public class Mushroom : MonoBehaviour
{
    MushroomMove mushroomMove;
    private void Awake()
    {
        mushroomMove = GetComponent<MushroomMove>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerAnimation>().EatMushroom();
            Destroy(gameObject);
        }
    }
}
