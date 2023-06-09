using UnityEngine;

public class CollisionMonster : MonoBehaviour
{
    [SerializeField]
    private float minDistanceToPress = 0.5f;

    private JumpController jumpController;
    private PlayerAnimation playerAnimation;
    private void Awake()
    {
        jumpController = GetComponent<JumpController>();
        playerAnimation = GetComponent<PlayerAnimation>();
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
        playerAnimation.Hit();
    }
}
