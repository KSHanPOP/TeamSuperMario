using UnityEngine;

public abstract class MonsterBase : MonoBehaviour
{    
    protected float monsterHeight;

    [SerializeField]
    protected bool isPressable;

    [SerializeField]
    protected SpriteRenderer spriteRenderer;

    private void Awake()
    {   
        monsterHeight = GetComponentInChildren<SpriteRenderer>().size.y;

        spriteRenderer.sortingOrder = (int)EnumSpriteLayerOrder.Monster;
    }

    public bool IsPressable(float posY, float minDistanceToPress)
    {
        if(!isPressable)
            return false;

        return posY > monsterHeight * minDistanceToPress + transform.position.y;
    }
    
}
