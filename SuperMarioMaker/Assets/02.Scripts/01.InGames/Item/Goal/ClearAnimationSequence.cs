using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearAnimationSequence : MonoBehaviour
{
    private GameObject player;    

    [SerializeField]
    private float maxFallSpeedWhileGrap = 5f;

    public void StartSequence(GameObject player)
    {
        this.player = player;

        MovementLimmiter.instance.CharacterCanMove = false;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<JumpController>().MaxFallSpeed = maxFallSpeedWhileGrap;

        player.GetComponent<PlayerAnimation>().GrapFlag();
        player.transform.position = new Vector3(transform.position.x - 0.5f, player.transform.position.y, 0f);
    }    

    public void EndSequence()
    {

    }
}
