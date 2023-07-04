using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;

public class Needle : MonoBehaviour
{
    private static LinkedList<Needle> Needles= new LinkedList<Needle>();

    private int hashPlay = Animator.StringToHash("Play");
    private int hashStop = Animator.StringToHash("Stop");

    [SerializeField]
    private Animator animator;

    public static void StartTest()
    {
        foreach (var needle in Needles)
        {
            if (needle != null)
                needle.Play();
        }
    }
    public static void StopTest()
    {
        foreach (var needle in Needles)
        {
            if (needle != null)
                needle.Stop();
        }
    }

    private void Awake()
    {
        Needles.AddLast(this);
    }

    private void Play()
    {
        animator.SetTrigger(hashPlay);
    }
    private void Stop()
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
