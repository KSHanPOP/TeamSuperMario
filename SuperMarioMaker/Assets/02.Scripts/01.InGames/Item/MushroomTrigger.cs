using UnityEngine;

public class MushroomTrigger : MonoBehaviour
{  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerAnimation>().EatMushroom();
            Destroy(transform.parent.gameObject);
        }
    }
}
