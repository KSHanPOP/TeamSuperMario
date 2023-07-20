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
    private GroundChecker groundChecker;

    private int stompCombo = 0;

    private void Awake()
    {
        jumpController = GetComponent<JumpController>();
        playerAnimation = GetComponent<PlayerAnimation>();
        groundChecker = GetComponent<GroundChecker>();

        playerState = GetComponentInChildren<PlayerState>();
    }

    private void FixedUpdate()
    {
        if(groundChecker.IsGround())
            stompCombo = 0;
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

        if(go.TryGetComponent(out MonsterBase monster) &&
            monster.IsAttackable(transform.position, minDistanceToPress))
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

        ScoreManager.Instance.GetComboScore(stompCombo++, go.transform.position);
        go.GetComponent<IPressable>().Press();

        if (!groundChecker.IsGround())
            jumpController.MonsterPressJump();
    }
    public void Hit()
    {
        if (gameObject.layer != invincibleLayer)
            playerAnimation.Hit();
    }
}
