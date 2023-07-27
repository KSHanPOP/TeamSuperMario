using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
    public int[] scores;

    public float[] differentialScoreZone;    

    private bool isStageClear = false;

    [SerializeField]
    private Flag flag;

    [SerializeField]
    private ClearAnimationSequence clearAnimation;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isStageClear)
            return;

        if(collision.CompareTag("Player"))
        {
            isStageClear = true;
            Camera.main.GetComponent<SleepObjectAwaker>().ClearMonsters();
            Castle.Instance.StageClear();

            SetScore(collision.transform.position.y);
            flag.GetScore();

            clearAnimation.StartSequence(collision.gameObject);            
        }                
    }

    private void SetScore(float playerHeight)
    {       
        float heightAvobeFloor = playerHeight - (transform.position.y + 0.5f);

        for(int i = 0; i < differentialScoreZone.Length; i++)
        {
            if(heightAvobeFloor < differentialScoreZone[i])
            {
                flag.Score = scores[i];                
                return;
            }
        }

        flag.Score = scores[scores.Length - 1];             
    }
    
}
