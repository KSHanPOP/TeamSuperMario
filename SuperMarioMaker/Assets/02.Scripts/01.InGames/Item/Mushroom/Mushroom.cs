using UnityEngine;
public class Mushroom : ItemBase
{
    private Rigidbody2D rb;

    [SerializeField]
    private float gravityScale;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    protected override void StartAction()
    {
        base.StartAction();
        GetComponent<MushroomMove>().enabled = true;
        rb.gravityScale = gravityScale;
        rb.isKinematic = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerAnimation>().EatMushroom();            
            Logger.Debug("eat");
            Destroy(gameObject);
        }
    }
}
