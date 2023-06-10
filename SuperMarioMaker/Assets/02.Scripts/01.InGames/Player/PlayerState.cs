using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public PlayerBase CurrState { get; set; }
    
    private MarioSmall marioSmall;
    
    private MarioBig marioBig;
    
    private MarioFire marioFire;

    private void Awake()
    {
        marioSmall = GetComponent<MarioSmall>();
        marioBig = GetComponent<MarioBig>();    
        marioFire = GetComponent<MarioFire>();

        CurrState = marioSmall;
    }

}
