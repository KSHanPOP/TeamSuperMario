using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearAnimationSequence : MonoBehaviour
{
    private int hashGoalSequenceEnd = Animator.StringToHash("GoalSequenceEnd");

    private GameObject player;

    private Rigidbody2D playerRb;

    private Coroutine animationCoroutine;

    [SerializeField]
    private float maxFallSpeedWhileGrap = 5f;

    [SerializeField]
    private float[] sequenceStepTime;

    [SerializeField]
    private Vector2 eventMoveValue;

    [SerializeField]
    private float offset = 5;

    public void StartSequence(GameObject player)
    {
        SoundManager.Instance.StopAll();
        SoundManager.Instance.PlaySFX("Flagpole");

        this.player = player;

        offset /= 16;

        MovementLimmiter.instance.CharacterCanMove = false;
        playerRb = player.GetComponent<Rigidbody2D>();
        playerRb.velocity = Vector2.zero;
        player.GetComponent<JumpController>().MaxFallSpeed = maxFallSpeedWhileGrap;

        player.GetComponent<PlayerAnimation>().GrapFlag();
        player.transform.position = new Vector3(transform.position.x - offset, player.transform.position.y, 0f);
    }    

    public void NextSequnce()
    {            
        animationCoroutine = StartCoroutine(AnimationCoroutine());
    }

    private void StepOne()
    {
        player.transform.position = new Vector3(transform.position.x + offset, player.transform.position.y, 0f);
        player.transform.localScale = new Vector3(-1, 1, 1);
    }
    private void StepTwo()
    {
        player.transform.localScale = Vector3.one;
        PlayerState.Instance.Animator.SetTrigger(hashGoalSequenceEnd);
        playerRb.velocity = eventMoveValue;
    }
    IEnumerator AnimationCoroutine()
    {
        yield return new WaitForSeconds(sequenceStepTime[0]);

        StepOne();

        yield return new WaitForSeconds(sequenceStepTime[1]);

        StepTwo();

        Vector2 maintainMoveValue;

        while (true)
        {
            maintainMoveValue.x = eventMoveValue.x;
            maintainMoveValue.y = playerRb.velocity.y;

            playerRb.velocity = maintainMoveValue;
            yield return null;
        }
    }
}
