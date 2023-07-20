using UnityEngine;
public class Mushroom : ItemBase
{
    private bool isAte;

    private Rigidbody2D rb;

    [SerializeField]
    private float gravityScale;

    [SerializeField]
    private MushroomMove mushroomMove;

    [SerializeField]
    private MoveColliderDetect moveColliderDetect;

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    protected override void StartAction()
    {
        base.StartAction();
        mushroomMove.enabled = true;
        moveColliderDetect.enabled = true;
        rb.gravityScale = gravityScale;
        rb.isKinematic = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAte)
            return;

        if (collision.CompareTag("Player"))
        {
            ScoreManager.Instance.GetScore(1000, transform.position);
            isAte = true;
            collision.GetComponent<PlayerAnimation>().EatMushroom();                        
            Destroy(gameObject);
        }
    }
}
