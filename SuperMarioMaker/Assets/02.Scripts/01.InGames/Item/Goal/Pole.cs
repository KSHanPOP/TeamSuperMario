using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Logger.Debug(collision.name);        
    }
}
