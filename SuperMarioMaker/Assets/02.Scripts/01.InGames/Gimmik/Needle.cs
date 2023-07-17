using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needle : StaticTile
{
    private int hashPlay = Animator.StringToHash("Play");
    private int hashStop = Animator.StringToHash("Stop");

    [SerializeField]
    private Animator animator;

    public override void Play()
    {
        animator.SetTrigger(hashPlay);
    }
    public override void Stop()
    {
        animator.SetTrigger(hashStop);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Invincible"))
            return;

        collision.collider.GetComponent<PlayerAnimation>().Hit();
    }
}
