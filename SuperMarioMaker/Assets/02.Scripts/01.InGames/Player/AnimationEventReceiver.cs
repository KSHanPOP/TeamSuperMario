using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    [SerializeField]
    private PlayerAnimation playerAnimation;
    public void OnTransformationComplete()
    {
        playerAnimation.OnTransformationComplete();
    }
    public void TryBackJump()
    {        
        playerAnimation.TryBackJump();
    }
}
