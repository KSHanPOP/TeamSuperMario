using UnityEngine;

public abstract class MonsterBase : MonoBehaviour
{    
    protected float monsterHeight;

    [SerializeField]
    protected bool isPressable;

    private void Awake()
    {   
        monsterHeight = GetComponentInChildren<SpriteRenderer>().size.y;        
    }

    public bool IsPressable(float posY, float minDistanceToPress)
    {
        if(!isPressable)
            return false;

        return posY > monsterHeight * minDistanceToPress + transform.position.y;
    }
    
}
