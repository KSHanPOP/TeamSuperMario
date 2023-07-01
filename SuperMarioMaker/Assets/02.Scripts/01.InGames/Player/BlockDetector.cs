using UnityEngine;

public class BlockDetector : MonoBehaviour
{
    [SerializeField]
    PlayerState playerState;

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private float blockDetectLength;
    public float BlockDetectLength { set { blockDetectLength = value; } }

    [SerializeField]
    private JumpController jumpController;

    private LayerMask platformLayer;

    private bool gizmo1;
    private bool gizmo2;

    private void Awake()
    {
        platformLayer = LayerMask.GetMask("Platform");
    }

    private void Update()
    {
        if (!playerState.IsAttckable)
            return;

        if (!jumpController.IsAttackableBlock)
            return;

        RaycastHit2D hit1 = Physics2D.Raycast(transform.position - offset, Vector2.up, blockDetectLength, platformLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + offset, Vector2.up, blockDetectLength, platformLayer);

        gizmo1 = hit1;
        gizmo2 = hit2;

        if (hit1 && hit2)
        {
            var isHit1Closest = Mathf.Abs(hit1.transform.position.x - transform.position.x) <= Mathf.Abs(hit2.transform.position.x - transform.position.x);

            HitBlock(isHit1Closest ? hit1.transform : hit2.transform);

            return;
        }

        if(hit1)
        {
            HitBlock(hit1.transform);
        }

        if(hit2)
        {
            HitBlock(hit2.transform);
        }
    }

    public void HitBlock(Transform transform)
    {
        if (!transform.TryGetComponent<Block>(out Block block))
            return;

        if (playerState.IsSmallState())
        {
            block.NormalHit();
        }
        else
            block.BigHit();

        jumpController.IsAttackableBlock = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmo1 || gizmo2 ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position - offset, transform.position - offset + Vector3.up * blockDetectLength);
        Gizmos.DrawLine(transform.position + offset, transform.position + offset + Vector3.up * blockDetectLength);
    }    
}
