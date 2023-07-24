using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private int hashGetScore = Animator.StringToHash("GetScore");

    public int Score { get; set; }

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private ClearAnimationSequence sequence;

    public void GetScore()
    {

        if (SceneLoader.Instance.State == SceneState.MainGame)
        {
            InGameManager.Instance.TimeStop();
        }

        animator.SetTrigger(hashGetScore);
    }

    public void FlagDown()
    {
        ScoreManager.Instance.GetScore(Score, transform.position);
        sequence.NextSequnce();

    }

}
