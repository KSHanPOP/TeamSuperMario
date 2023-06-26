using UnityEngine;

public class CollisionMonster : MonoBehaviour
{
    [SerializeField]
    private float minDistanceToPress = 0.5f;

    [SerializeField]
    int invincibleLayer;

    private PlayerState playerState;

    private JumpController jumpController;
    private PlayerAnimation playerAnimation;
    private void Awake()
    {
        jumpController = GetComponent<JumpController>();
        playerAnimation = GetComponent<PlayerAnimation>();

        playerState = GetComponentInChildren<PlayerState>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionCheck(collision.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.layer == invincibleLayer)
            CollisionCheck(collision.gameObject);
    }

    private void CollisionCheck(GameObject go)
    {
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
        if (!playerState.IsAttckable)
            return;

        go.GetComponent<AttackedPressed>().OnAttack();
        jumpController.DoJump();
    }
    public void Hit()
    {
        if (gameObject.layer != invincibleLayer)
            playerAnimation.Hit();
    }
}
