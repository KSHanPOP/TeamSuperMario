using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearAnimationSequence : MonoBehaviour
{
    private int hashGoalSequenceEnd = Animator.StringToHash("GoalSequenceEnd");

    private GameObject player;

    private Rigidbody2D playerRb;

    private Coroutine stepTwoCoroutine;

    [SerializeField]
    private float maxFallSpeedWhileGrap = 5f;

    [SerializeField]
    private float[] sequenceStepTime;

    [SerializeField]
    private Vector2 eventMoveValue;

    public void StartSequence(GameObject player)
    {
        this.player = player;

        MovementLimmiter.instance.CharacterCanMove = false;
        playerRb = player.GetComponent<Rigidbody2D>();
        playerRb.velocity = Vector2.zero;
        player.GetComponent<JumpController>().MaxFallSpeed = maxFallSpeedWhileGrap;

        player.GetComponent<PlayerAnimation>().GrapFlag();
        player.transform.position = new Vector3(transform.position.x - 0.5f, player.transform.position.y, 0f);
    }    

    public void NextSequnce()
    {
        Invoke(nameof(StepOne), sequenceStepTime[0]);        
        stepTwoCoroutine = StartCoroutine(StepTwoCoroutine(sequenceStepTime[1]));
    }

    private void StepOne()
    {
        player.transform.position = new Vector3(transform.position.x + 0.5f, player.transform.position.y, 0f);
        player.transform.localScale = new Vector3(-1, 1, 1);
    }
    private void StepTwo()
    {
        player.transform.localScale = Vector3.one;
        PlayerState.Instance.Animator.SetTrigger(hashGoalSequenceEnd);
        playerRb.velocity = eventMoveValue;
    }
    IEnumerator StepTwoCoroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

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
