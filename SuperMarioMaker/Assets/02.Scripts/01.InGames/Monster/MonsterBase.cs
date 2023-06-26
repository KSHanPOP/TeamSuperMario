using UnityEngine;

public abstract class MonsterBase : MonoBehaviour
{    
    protected float monsterHeight;

    [SerializeField]
    protected bool isPressable;

    [SerializeField]
    protected SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {   
        monsterHeight = spriteRenderer.size.y;

        spriteRenderer.sortingOrder = (int)EnumSpriteLayerOrder.Monster;
    }
    public virtual bool IsPressable(float posY, float minDistanceToPress)
    {
        if(!isPressable)
            return false;

        return posY > monsterHeight * minDistanceToPress + transform.position.y;
    }
    
}
