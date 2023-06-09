using UnityEngine;

public class CollisionMonster : MonoBehaviour
{
    [SerializeField]
    private float minDistanceToPress = 0.5f;

    JumpController jumpController;
    private void Awake()
    {
        jumpController = GetComponent<JumpController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var go = collision.gameObject;

        if (!go.CompareTag("Monster"))
            return;

        if (go.GetComponent<MonsterBase>().IsPressable(transform.position.y, minDistanceToPress))
        {
            Attack(go);
            return;
        }

        Hit();
    }
     private void Attack(GameObject go)
    {
        go.GetComponent<IAttackable>().OnAttack();
        jumpController.DoJump();
    }
    private void Hit()
    {
        Logger.Debug("hit");        
    }
}
