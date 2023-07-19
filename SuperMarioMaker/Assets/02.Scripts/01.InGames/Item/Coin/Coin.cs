using UnityEngine;

public class Coin : MonoBehaviour, IShakeable
{
    [SerializeField]
    private SpinCoin spinCoin;

    private SpriteRenderer spriteRenderer;

    public void Shake(Vector2 _)
    {
        Instantiate(spinCoin, transform.position, Quaternion.identity, transform.parent).StartInstance(transform.position + Vector3.up);
        SoundManager.Instance.PlaySFX("Coin");
        Destroy(gameObject);        
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = (int)EnumSpriteLayerOrder.Item;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        SoundManager.Instance.PlaySFX("Coin");

        ScoreManager.Instance.GetCoin(1);

        Destroy(gameObject);
    }  
}
