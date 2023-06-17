using UnityEngine;
public class MarioFire : MarioBig
{   
    private MarioBig marioBig;
    protected override void Awake()
    {
        base.Awake();        
        marioBig = GetComponent<MarioBig>();
    }
}
