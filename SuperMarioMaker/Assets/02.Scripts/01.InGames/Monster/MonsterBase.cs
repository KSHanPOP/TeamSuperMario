using UnityEngine;

public abstract class MonsterBase : MonoBehaviour
{    
    protected float monsterHeight;

    [SerializeField]
    protected bool isAttackable;

    [SerializeField]
    protected SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {   
        monsterHeight = spriteRenderer.size.y;

        spriteRenderer.sortingOrder = (int)EnumSpriteLayerOrder.Monster;
    }
    public virtual bool IsAttackable(Vector2 pos, float minDistanceToPress)
    {
        if(!isAttackable)
            return false;

        return pos.y > monsterHeight * minDistanceToPress + transform.position.y;
    }    
}
