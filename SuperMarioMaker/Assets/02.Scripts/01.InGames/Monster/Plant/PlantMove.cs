using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantMove : MonoBehaviour
{
    private int hashIsPlayerClose = Animator.StringToHash("IsPlayerClose");
    private int hashIsMoveEnded = Animator.StringToHash("IsMoveEnded");

    private Transform playerTransform;
    private float middlePosX;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject trigger;

    [SerializeField]
    LayerMask invincibleLayer;
    [SerializeField]
    LayerMask monsterLayer;

    int invincibleLayerToInt;
    int monsterLayerToInt;

    private bool startAct = false;

    private void Awake()
    {
        invincibleLayerToInt = (int)Mathf.Log(invincibleLayer.value, 2);        
        monsterLayerToInt = (int)Mathf.Log(monsterLayer.value, 2);        
    }

    private void Start()
    {
        playerTransform = PlayerState.Instance.transform;
        middlePosX = transform.position.x + 0.5f;
    }
    private void Update()
    {
        float playerDistance = Mathf.Abs(playerTransform.position.x - middlePosX);

        if (!startAct && playerDistance < 10f)
        {
            startAct = true;
            animator.enabled = true;
        }

        animator.SetBool(hashIsPlayerClose, playerDistance < 1.5f);
    }

    public void MoveEnd()
    {
        animator.SetTrigger(hashIsMoveEnded);
    }
    public void DigIn()
    {
        SleepCollision();
    }
    public void DigOut()
    {        
        trigger.layer = monsterLayerToInt;
    }
    public void SleepCollision()
    {
        trigger.layer = invincibleLayerToInt;
    }    
}
