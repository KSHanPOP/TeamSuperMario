using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    [SerializeField]
    private PlayerAnimation playerAnimation;

    [SerializeField]
    private MarioFire marioFire;
    public void OnTransformationComplete()
    {
        playerAnimation.OnTransformationComplete();
    }
    public void TryBackJump()
    {        
        playerAnimation.TryBackJump();
    }

    public void Fired()
    {
        marioFire.Fired();
    }
}
