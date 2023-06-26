using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantMove : MonoBehaviour
{
    private int hashIsPlayerClose = Animator.StringToHash("IsPlayerClose");
    private int hashIsMoveEnded = Animator.StringToHash("IsMoveEnded");

    private Transform playerTransform;

    [SerializeField]
    private Animator animator;

    private void Start()
    {
        playerTransform = PlayerState.Instance.transform;
    }
    private void Update()
    {
        animator.SetBool(hashIsPlayerClose, Mathf.Abs(playerTransform.position.x - transform.position.x) < 1.5f);
    }

    public void MoveEnd()
    {
        animator.SetTrigger(hashIsMoveEnded);
    }
}
