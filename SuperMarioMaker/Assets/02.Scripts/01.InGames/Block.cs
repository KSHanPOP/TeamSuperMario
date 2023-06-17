using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    BlockType startBlock;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}

public enum BlockType
{
    None = -1,
    Normal = 0,
    Question,
    Used,
    Transparent,
}
