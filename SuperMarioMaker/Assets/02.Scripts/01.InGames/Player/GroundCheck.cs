using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private bool isGround;
    public bool IsGround { get { return isGround; } }

    //private Collider2D groundChecker;
    //private void Awake()
    //{        
    //    var colilders = GetComponents<Collider2D>();
    //    foreach (var collider in colilders)
    //    {
    //        if (collider.isTrigger)
    //            groundChecker = collider;
    //    }
    //}
    private void OnTriggerStay2D(Collider2D collision)
    {
        isGround = collision.CompareTag("Platform");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isGround = false;        
    }
    private void Update()
    {
        Logger.Debug(isGround);
    }
}
