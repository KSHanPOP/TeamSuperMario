using UnityEngine;

public class BlockCrashAnimation : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D[] fragments;

    [SerializeField]
    private float gravityScale;

    [SerializeField]
    private Vector2 force1;

    [SerializeField]
    private Vector2 force2;

    [SerializeField]
    private float torque;

    private void Awake()
    {
        foreach (var fragment in fragments)
        {
            fragment.GetComponent<SpriteRenderer>().sortingOrder = (int)EnumSpriteLayerOder.MonsterDie;
        }
    }
    public void OnCrash()
    {
        foreach (var fragment in fragments)
        {
            fragment.gravityScale = gravityScale;
        }

        fragments[0].AddForce(force1, ForceMode2D.Impulse);
        fragments[0].AddTorque(torque, ForceMode2D.Impulse);
        force1.x *= -1;
        fragments[1].AddForce(force1, ForceMode2D.Impulse);
        fragments[1].AddTorque(-torque, ForceMode2D.Impulse);

        fragments[2].AddForce(force2, ForceMode2D.Impulse);
        fragments[2].AddTorque(torque, ForceMode2D.Impulse);
        force2.x *= -1;
        fragments[3].AddForce(force2, ForceMode2D.Impulse);
        fragments[3].AddTorque(-torque, ForceMode2D.Impulse);

        Destroy(gameObject, 5f);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            OnCrash();
        }
    }
}
