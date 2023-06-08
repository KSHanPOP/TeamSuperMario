using UnityEngine;

public class PressMonster : MonoBehaviour
{
    [SerializeField]
    private float needHeightDistanceToPress = 0.5f;

    JumpController jumpController;
    private void Awake()
    {
        jumpController = GetComponent<JumpController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Monster"))
            return;

        if (transform.position.y - collision.transform.position.y < needHeightDistanceToPress)
            return;

        collision.GetComponent<IAttackable>().OnAttack();
        jumpController.DoJump();
    }
}
