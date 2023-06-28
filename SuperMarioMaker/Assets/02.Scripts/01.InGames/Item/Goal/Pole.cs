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
            Castle.Instance.StageClear();

            flag.GetScore();
            GetScore(collision.transform.position.y);

            clearAnimation.StartSequence(collision.gameObject);            
        }                
    }

    private void GetScore(float playerHeight)
    {       
        float heightAvobeFloor = playerHeight - (transform.position.y + 0.5f);

        for(int i = 0; i < differentialScoreZone.Length; i++)
        {
            if(heightAvobeFloor < differentialScoreZone[i])
            {
                Logger.Debug($"get score{scores[i]}");
                return;
            }
        }

        Logger.Debug($"get score{scores[scores.Length - 1]}");
    }
    
}
