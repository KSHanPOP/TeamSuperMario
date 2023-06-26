using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private int hashGetScore = Animator.StringToHash("GetScore");

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private ClearAnimationSequence sequence;

    public void GetScore()
    {
        animator.SetTrigger(hashGetScore);
    }

    public void FlagDown()
    {
        sequence.NextSequnce();
    }

}
