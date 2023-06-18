using UnityEngine;
public class Block : MonoBehaviour
{
    [SerializeField]
    private bool isTransparent;

    [SerializeField]
    private Sprite usedSprite;

    [SerializeField]
    private EnumItems itemType;

    [SerializeField]
    private int coinCount;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        SetStartSprite();
    }
    private void SetStartSprite()
    {
        if (isTransparent)
            spriteRenderer.sprite = null;
    }
}
